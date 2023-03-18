using System.Collections.Generic;

namespace inc.stu.SystemUI.MenuBar
{
    public class MenuBarBase
    {
        public string Name;
        public List<MenuBarItem> Items = new();
        public List<MenuBarBase> ChildMenus = new();

        public virtual void Initialize()
        {
            MenuBarManager.Instance.AppendMenu(this);
        }
    }
}