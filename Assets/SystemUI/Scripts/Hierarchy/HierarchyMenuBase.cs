using System.Collections.Generic;

namespace Previz.Hierarchy
{
    public class HierarchyMenuBase
    {
        /// <summary>
        /// メニューを表示する際に右クリックしたアイテム情報
        /// </summary>
        protected HierarchyItemData _currentItemData;

        public string Name;
        public List<HierarchyMenuItem> Items = new();
        public List<HierarchyMenuBase> ChildMenu = new();

        public void SetHierarchyItemData(HierarchyItemData data)
        {
            _currentItemData = data;
        }
    }
}