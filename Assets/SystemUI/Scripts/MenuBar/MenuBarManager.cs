using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI.MenuBar
{
    public class MenuBarManager : MonoBehaviourSingleton<MenuBarManager>
    {
        [SerializeField] private MenuBarItemView _menuBarItemViewPrefab;
        [SerializeField] private PopupMenuView _popupMenuViewPrefab;
        [SerializeField] private PopupMenuItemView _popupMenuViewItemPrefab;
        [SerializeField] private RectTransform _rootNode;

        public MenuBarItemView MenuBarItemViewPrefab => _menuBarItemViewPrefab;
        public PopupMenuView PopupMenuViewPrefab => _popupMenuViewPrefab;
        public PopupMenuItemView PopupMenuItemViewPrefab => _popupMenuViewItemPrefab;
        
        /// <summary>
        /// メニュー外のクリックを受け取るためのbutton
        /// </summary>
        [SerializeField] private Button _backgroundButton;

        private List<MenuBarBase> _menus = new();
        private List<MenuBarItemView> _menuItems = new();
        private List<IDisposable> _disposables = new();
        private GameObject _instantiatedPopupMenu;
        private MenuBarItemView _selectedMenuItem;
        private bool _isSelecting;
        
        protected override void Init()
        {
            foreach (Transform c in _rootNode)
            {
                Destroy(c.gameObject);
            }
            
            _backgroundButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (_instantiatedPopupMenu != null)
                {
                    Destroy(_instantiatedPopupMenu);
                    _selectedMenuItem.Deactivate();
                }

                _backgroundButton.gameObject.SetActive(false);
                _isSelecting = false;
            }).AddTo(this);
            _backgroundButton.gameObject.SetActive(false);
        }

        protected override void Deinit()
        {
            foreach (var disposable in _disposables) disposable.Dispose();
        }

        public void AppendMenu(MenuBarBase menu)
        {
            _menus.Add(menu);
        }

        /// <summary>
        /// _menusにAddされたメニューをInstantiateしイベントをSubscribeする
        /// </summary>
        public void UpdateMenu()
        {
            InstantiateMenuItems();
            BindMenuItemEvents();

            // 大元の常に表示されているMenu
            void InstantiateMenuItems()
            {
                foreach (var menu in _menus)
                {
                    var menuItem = Instantiate(_menuBarItemViewPrefab, _rootNode);
                    _menuItems.Add(menuItem);
                    menuItem.Setup(menu.Name);
                }
            }

            void BindMenuItemEvents()
            {
                
                // 大元のメニューの個数分
                for (var i = 0; i < _menuItems.Count; i++)
                {
                    var index = i;  // capture
                    
                    // 大元のメニューがクリックされたとき
                    _menuItems[index].OnClick.Subscribe(_ =>
                    {
                        // 既に出しているPopupMenuをDestroyしてリターンする
                        if (_instantiatedPopupMenu != null)
                        {
                            _isSelecting = false;
                            _backgroundButton.gameObject.SetActive(false);
                            Destroy(_instantiatedPopupMenu);
                            _selectedMenuItem.Deactivate();
                            _instantiatedPopupMenu = null;
                            return;
                        }

                        // PopupMenuをInstantiateし、ClickEventをBindする
                        _isSelecting = true;
                        SetupPopupMenu(_menus[index], _menuItems[index]);
                        _backgroundButton.gameObject.SetActive(true);
                    }).AddTo(_disposables);
                    
                    _menuItems[index].OnMouseEnter.Subscribe(_ =>
                    {
                        // 何も選択されていない場合はMouseEnterしても何もしない
                        if (!_isSelecting) return;

                        // 既に出しているPopupMenuをDestroyする
                        if (_instantiatedPopupMenu != null)
                        {
                            Destroy(_instantiatedPopupMenu);
                            _selectedMenuItem.Deactivate();
                        }

                        SetupPopupMenu(_menus[index], _menuItems[index]);
                    }).AddTo(_disposables);
                }
            }

            void ClickPopupMenu(PopupMenuView menu)
            {
                _isSelecting = false;
                _backgroundButton.gameObject.SetActive(false);
                Destroy(menu.gameObject);
                _selectedMenuItem.Deactivate();
            }

            void SetupPopupMenu(MenuBarBase menu, MenuBarItemView menuBarItemView)
            {
                var popupMenu = Instantiate(_popupMenuViewPrefab, menuBarItemView.PopupMenuRootRect);
                popupMenu.Setup(menu);
                
                // ポップアップメニューの中の何かしらが押されたとき
                popupMenu.OnClick.Subscribe(_ => ClickPopupMenu(popupMenu)).AddTo(popupMenu);
                
                _instantiatedPopupMenu = popupMenu.gameObject;
                _selectedMenuItem = menuBarItemView;
                _selectedMenuItem.Activate();
            }
        }

        public void ResetMenu()
        {
            foreach (var item in _menuItems)
            {
                Destroy(item.gameObject);
            }

            _menus.Clear();
            _menuItems.Clear();
        }
    }
}