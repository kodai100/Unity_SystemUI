using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Previz.Hierarchy
{
    public class HierarchyView : UIBehaviour
    {
        [SerializeField] private HierarchyItemView _itemView;
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private HierarchyMenuController _hierarchyMenuController;

        private HierarchyItemView _selectingView;
        private HierarchyMenuBase _rightClickMenu;
        private HierarchyItemData _mouseOverItemData;
        private HierarchyItemView _mouseOverItemView;
        private bool _isDraggingItem;
        private bool _isMouseOverBetweenItem;
        private bool _isEnterHierarchyArea;

        private List<HierarchyItemView> _instantiatedItems = new();

        private Subject<HierarchyItemDropInEvent> _onItemDroppedInEvent = new();
        private Subject<HierarchyItemInsertEvent> _onItemInsertEvent = new();
        private Subject<HierarchyItemData> _onClickItem = new();
        private Subject<HierarchyItemData> _onRightClickItem = new();
        public IObservable<HierarchyItemData> OnLeftClickItem => _onClickItem;
        public IObservable<HierarchyItemData> OnRightClickItem => _onRightClickItem;
        public IObservable<HierarchyItemDropInEvent> OnItemDroppedIn => _onItemDroppedInEvent;
        public IObservable<HierarchyItemInsertEvent> OnInsertItem => _onItemInsertEvent;

        public void Initialize(List<HierarchyItemData> data, HierarchyMenuBase rightClickMenu, Camera uiCamera)
        {
            _rightClickMenu = rightClickMenu;
            _hierarchyMenuController.SetMenu(rightClickMenu);
            InitializeItems(data, _rootRectTransform);

            // メニュー表示時にメニュー外のクリックを監視する
            Observable.EveryUpdate()
                .Where(_ => _hierarchyMenuController.isActiveAndEnabled)
                .Where(_ => Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                .Where(_ => !_hierarchyMenuController.CheckCollide())
                .Subscribe(_ =>
                {
                    _hierarchyMenuController.DestroyMenu();
                }).AddTo(this);
            
            this.OnPointerEnterAsObservable()
                .Where(_ => _isDraggingItem)
                .Subscribe(_ => _isEnterHierarchyArea = true).AddTo(this);
            
            this.OnPointerExitAsObservable()
                .Where(_ => _isDraggingItem)
                .Subscribe(_ => _isEnterHierarchyArea = false).AddTo(this);
        }

        public void AddItem(HierarchyItemData item, int index, HierarchyItemView parentView = default)
        {
            var list = new List<HierarchyItemData> { item };

            Transform parentTransform = null;
            
            // Parentがいない場合、ルートに設置する
            if (parentView == null)
            {
                InitializeItems(list, _rootRectTransform);
                parentTransform = _rootRectTransform.transform;
            }
            else
            {
                // ParentのChildrenRootRectに設置する
                parentView.ShowChildren();
                InitializeItems(list, parentView.ChildrenRootRect, parentView.Depth + 1);
                parentTransform = parentView.ChildrenRootRect;
            }
            
            var siblings = parentTransform.GetComponentsInChildren<HierarchyItemView>().Where(x => x.transform.parent == parentTransform).ToArray();
            var addedItemView = siblings.SingleOrDefault(x => x.ItemId == item.Id);
            addedItemView!.transform.SetSiblingIndex(index);
        }

        public void DeleteItem(Guid itemId)
        {
            var children = GetComponentsInChildren<HierarchyItemView>();
            var target = children.SingleOrDefault(x => x.ItemId == itemId);
            if (target == null)
            {
                Debug.LogError("削除しようとしているアイテムのViewが存在しません。");
                return;
            }

            Destroy(target.gameObject);
        }

        // 実際のゲームオブジェクト操作
        public IEnumerator InsertItem(HierarchyItemData movedItem, Guid? parentItemId, int index)
        {
            DeleteItem(movedItem.Id);

            // 削除の反映を待つ
            yield return null;

            var parentView = GetComponentsInChildren<HierarchyItemView>().SingleOrDefault(x => x.ItemId == parentItemId);
            AddItem(movedItem, index, parentView);

            if (parentView != null) parentView.HideHoverImage();
            UpdateSelectItem(movedItem.Id);
        }

        public void UpdateSelectItem(Guid id)
        {
            if (_selectingView != null) _selectingView.UnSelect();

            var view = GetComponentsInChildren<HierarchyItemView>()
                .SingleOrDefault(x => x.ItemId == id);
            if (view == null)
            {
                Debug.LogError($"Id : {id} のHierarchyItemViewがありません。");
                return;
            }

            view.Select();
            _selectingView = view;
        }

        public void RenameItem(Guid id, string newName)
        {
            var view = GetComponentsInChildren<HierarchyItemView>()
                .SingleOrDefault(x => x.ItemId == id);
            if (view == null)
            {
                Debug.LogError($"Id : {id}, 変更しようとしているHierarchyItemがありません。");
                return;
            }

            view.RenameItem(newName);
        }

        /// <summary>
        /// presenterへ公開するためのイベントの生成
        /// </summary>
        /// <param name="data"></param>
        private void HandlingItemDrop(HierarchyItemData data)
        {
            var views = GetComponentsInChildren<HierarchyItemView>();

            if (_mouseOverItemData == null)
            {
                // ヒエラルキー外なら何もしない
                if (!_isEnterHierarchyArea) return;

                // ヒエラルキー内ならルートへのアイテム移動として処理
                var aboveView = views.Last(x => x.transform.parent == _rootRectTransform);
                _onItemInsertEvent.OnNext(new HierarchyItemInsertEvent()
                {
                    AboveItemId = aboveView.ItemId,
                    MoveItemId = data.Id
                });
                return;
            }

            var mouseOverView = views.SingleOrDefault(x => x.ItemId == _mouseOverItemData.Id);
            if (mouseOverView == null)
            {
                Debug.LogError("挿入しようとしているアイテムの親アイテムが存在しません。");
                return;
            }

            HierarchyItemView parent = null;
            // アイテムの下（間）に挿入しようとしている場合
            if (_isMouseOverBetweenItem)
            {
                // 子を表示中の場合、InsertEventではなくMoveEventとして処理する
                if (mouseOverView.IsShowChildren)   // TODO: そのさらに親が開いてるかどうか見ないとだめじゃない？
                {
                    Debug.Log("Hi");
                    _onItemDroppedInEvent.OnNext(new HierarchyItemDropInEvent()
                    {
                        MoveItemId = data.Id,
                        ParentItemId = mouseOverView.ItemId,
                        Index = 0
                    });
                    return;
                }

                parent = mouseOverView.transform.parent.GetComponent<HierarchyItemView>();
                // parentがいない = ルートに挿入の場合
                if (parent == null)
                {
                    Debug.Log(mouseOverView.ItemId);
                    _onItemInsertEvent.OnNext(new HierarchyItemInsertEvent()
                    {
                        AboveItemId = mouseOverView.ItemId,
                        MoveItemId = data.Id
                    });
                    return;
                }

                _onItemInsertEvent.OnNext(new HierarchyItemInsertEvent()
                {
                    AboveItemId = parent.ItemId,
                    MoveItemId = data.Id
                });
                return;
            }

            // 要素間ではない場合、mouseOverViewを直接親として扱う
            parent = mouseOverView;
            _onItemDroppedInEvent.OnNext(new HierarchyItemDropInEvent()
            {
                MoveItemId = data.Id,
                ParentItemId = parent.ItemId,
                Index = parent.ChildrenRootRect.transform.childCount
            });
        }

        private void InitializeItems(List<HierarchyItemData> items, RectTransform parentTransform, int depth = 0)
        {
            foreach (var itemData in items)
            {
                var view = Instantiate(_itemView, parentTransform);
                view.Setup(itemData, depth);
                _instantiatedItems.Add(view);
                
                // 通常のクリック
                view.OnLeftClick.Subscribe(_ => _onClickItem.OnNext(itemData)).AddTo(this);

                // 右クリック
                view.OnRightClick.Subscribe(_ =>
                    {
                        _hierarchyMenuController.InstantiateMenu(itemData, _rightClickMenu);
                        _onRightClickItem.OnNext(itemData);
                    }
                ).AddTo(this);

                // トグルボタンのクリック
                view.OnClickToggleIcon.Subscribe(_ =>
                {
                    if (view.IsShowChildren) view.HideChildren();
                    else view.ShowChildren();
                }).AddTo(this);

                // Drag操作
                view.OnBeginDrag.Subscribe(_ =>
                {
                    _isEnterHierarchyArea = true;
                    _isDraggingItem = true;
                }).AddTo(this);
                view.OnEndDrag.Subscribe(x =>
                {
                    HandlingItemDrop(x);
                    _isDraggingItem = false;
                    if (_mouseOverItemView != null && _mouseOverItemView != view)
                    {
                        _mouseOverItemView.HideHoverImage();
                        _mouseOverItemView.HideUnderAreaImage();
                    }
                    if (_selectingView == view) view.Select();
                }).AddTo(this);

                // Drag時のMouseEnter, Exit関連の監視
                view.OnEnterTextArea
                    .Where(_ => _isDraggingItem)
                    .Subscribe(_ =>
                    {
                        _mouseOverItemData = itemData;
                        _mouseOverItemView = view;
                        view.ShowHoverImage();
                    })
                    .AddTo(this);
                view.OnExitTextArea
                    .Where(_ => _isDraggingItem)
                    .Subscribe(_ =>
                    {
                        if (_mouseOverItemData != itemData) return;
                        _mouseOverItemData = null;
                        _mouseOverItemView = null;
                        view.HideHoverImage();
                    })
                    .AddTo(this);
                view.OnEnterUnderArea
                    .Where(_ => _isDraggingItem)
                    .Subscribe(_ =>
                    {
                        _isMouseOverBetweenItem = true;
                        _mouseOverItemData = itemData;
                        _mouseOverItemView = view;
                        view.ShowUnderAreaImage();
                    }).AddTo(this);
                view.OnExitUnderArea
                    .Where(_ => _isDraggingItem)
                    .Subscribe(_ =>
                    {
                        _isMouseOverBetweenItem = false;
                        if (_mouseOverItemData != itemData) return;
                        _mouseOverItemData = null;
                        _mouseOverItemView = null;
                    }).AddTo(this);

                // 子要素が存在する場合は再帰的にこの関数を呼び出す
                var hasChild = itemData.ChildrenItems?.Count > 0;
                if (hasChild)
                {
                    InitializeItems(itemData.ChildrenItems, view.ChildrenRootRect, depth + 1);
                }

                // 初期化時はルート以外は隠して表示する
                if (depth == 0 && hasChild)
                {
                    view.HideChildren();
                }
            }
        }
    }
}