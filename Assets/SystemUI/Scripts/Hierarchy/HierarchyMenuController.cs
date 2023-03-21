using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Previz.Hierarchy
{
    /// <summary>
    /// ヒエラルキーメニューの管理を行うクラス
    /// </summary>
    public class HierarchyMenuController : MonoBehaviour
    {
        [SerializeField] private HierarchyMenuView _hierarchyMenuView;
        [SerializeField] private HierarchyMenuItemView _hierarchyMenuItemView;
        [SerializeField] private RectTransform _rootRect;

        [SerializeField] private Camera _uiCamera;
        
        private HierarchyMenuBase _menu;
        private GameObject _instantiatedPopupMenu;
        private const float Padding = 2;

        private List<HierarchyMenuItemView> _instantiatedMenuItemsForCollisionCheck = new();
        
        public void SetMenu(HierarchyMenuBase menu)
        {
            _menu = menu;
        }

        public bool CheckCollide()
        {
            return _instantiatedMenuItemsForCollisionCheck.Aggregate(false, (current, m) => current | m.IsMouseInside);
        }

        public void InstantiateMenu(HierarchyItemData data, HierarchyMenuBase rightClickMenu)
        {
            _menu.SetHierarchyItemData(data);

            if (_instantiatedPopupMenu != null)
            {
                Destroy(_instantiatedPopupMenu);
                _instantiatedPopupMenu = null;
            }
            
            var menuView = Instantiate(_hierarchyMenuView, _rootRect);
            AdjustMenuPosition(menuView);
            SetupMenuView(menuView, rightClickMenu);
            
            _instantiatedPopupMenu = menuView.gameObject;
        }

        public void DestroyMenu()
        {
            Destroy(_instantiatedPopupMenu);
            _instantiatedPopupMenu = null;
            _instantiatedMenuItemsForCollisionCheck.Clear();
        }

        // 右クリックされた位置に合わせてポジションを変える
        private void AdjustMenuPosition(HierarchyMenuView menuView)
        {
            var menuRect = menuView.GetComponent<RectTransform>();
            var mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(menuRect, mousePosition, null, out var localPoint);    // RenderModeがScreenSpaceCameraの場合はCameraを指定する必要があり、Overlayの場合はnullを指定する
            menuRect.anchoredPosition = localPoint;
        }

        private void SetupMenuView(HierarchyMenuView menuView, HierarchyMenuBase menuBase)
        {
            var menuRootRect = menuView.GetComponent<RectTransform>();
            foreach (var item in menuBase.Items)
            {
                var view = Instantiate(_hierarchyMenuItemView, menuRootRect);
                view.Setup(item.Name);
                view.OnClick.Subscribe(_ =>
                {
                    item.Action.Invoke();
                    DestroyMenu();
                }).AddTo(this);
                view.OnPointerEnter.Subscribe(_ =>
                {
                    menuView.CheckInstantiatedChildMenu();
                }).AddTo(this);
                
                _instantiatedMenuItemsForCollisionCheck.Add(view);
            }

            foreach (var childMenu in menuBase.ChildMenu)
            {
                var view = Instantiate(_hierarchyMenuItemView, menuRootRect);
                view.Setup(childMenu.Name);
                view.OnPointerEnter.Subscribe(_ =>
                {
                    menuView.CheckInstantiatedChildMenu();

                    var instantiatedChildMenu = Instantiate(_hierarchyMenuView, view.transform);
                    SetupMenuView(instantiatedChildMenu, childMenu);

                    var rect = view.GetComponent<RectTransform>();
                    var childRect = instantiatedChildMenu.GetComponent<RectTransform>();
                    childRect.anchoredPosition = new Vector2(rect.sizeDelta.x + Padding, childRect.anchoredPosition.y);

                    instantiatedChildMenu.OnClick.Subscribe(_ => DestroyMenu()).AddTo(instantiatedChildMenu);
                    menuView.SetInstantiatedChildMenu(instantiatedChildMenu);
                }).AddTo(this);
            }

            StartCoroutine(menuView.ApplyColliderSize());
        }
    }
}