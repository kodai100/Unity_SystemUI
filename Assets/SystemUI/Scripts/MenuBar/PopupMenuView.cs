using System;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI.MenuBar
{
    public class PopupMenuView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootNode;
        [SerializeField] private float _padding;

        private readonly Subject<Unit> _onClickSubject = new();
        private bool _isChildMenuDisplaying;
        private GameObject _childMenu;

        public IObservable<Unit> OnClick => _onClickSubject;

        public void Setup(MenuBarBase menu)
        {
            
            // アイテム
            foreach (var item in menu.Items)
            {
                var view = Instantiate(MenuBarManager.Instance.PopupMenuItemViewPrefab, _rootNode);
                view.Setup(item.Name);
                
                view.OnClick.Subscribe(_ =>
                {
                    item.Action.Invoke();
                    _onClickSubject.OnNext(Unit.Default);
                }).AddTo(this);
                
                view.OnPointerEnter.Subscribe(_ =>
                {
                    if (_isChildMenuDisplaying)
                    {
                        Destroy(_childMenu);
                        _isChildMenuDisplaying = false;
                    }
                }).AddTo(this);
            }

            // 子要素を持ったアイテム
            foreach (var childMenu in menu.ChildMenus)
            {
                var view = Instantiate(MenuBarManager.Instance.PopupMenuItemViewPrefab, _rootNode);
                view.Setup(childMenu.Name, true);
                
                // TODO: すでに子要素を出したあとでもPointerEnterが検出されてしまっているので
                view.OnPointerEnter.Subscribe(_ =>
                {
                    if (_isChildMenuDisplaying)
                    {
                        Destroy(_childMenu);
                        _isChildMenuDisplaying = false;
                    }
                    
                    var childPopupMenu = Instantiate(MenuBarManager.Instance.PopupMenuViewPrefab, view.transform);
                    childPopupMenu.Setup(childMenu);

                    var rect = view.GetComponent<RectTransform>();
                    var childRect = childPopupMenu.GetComponent<RectTransform>();
                    childRect.anchoredPosition = new Vector2(rect.sizeDelta.x + _padding, childRect.anchoredPosition.y);

                    childPopupMenu.OnClick.Subscribe(_ => _onClickSubject.OnNext(Unit.Default)).AddTo(childPopupMenu);
                    _isChildMenuDisplaying = true;
                    _childMenu = childPopupMenu.gameObject;
                }).AddTo(this);
            }
        }
    }
}