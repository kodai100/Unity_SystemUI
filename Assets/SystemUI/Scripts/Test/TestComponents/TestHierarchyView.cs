using System;
using System.Linq;
using Previz.Hierarchy;
using UniRx;
using UnityEngine;


namespace inc.stu.SystemUI.Tests
{
    
    public class TestHierarchyView : MonoBehaviour
    {
        [SerializeField] private HierarchyView _hierarchyView;
        [SerializeField] private Camera _uiCamera;
        
        public IObservable<HierarchyItemDropInEvent> OnItemMoved => _hierarchyView.OnDropInItem;
        public IObservable<HierarchyItemInsertEvent> OnItemInserted => _hierarchyView.OnInsertItem;
        public IObservable<HierarchyItemData> OnSelectedItem => _hierarchyView.OnLeftClickItem.Merge(_hierarchyView.OnRightClickItem);

        public void Setup(HierarchyEntity[] entities, HierarchyMenuBase hierarchyMenu)
        {
            var hierarchyData = entities.Select(x => new HierarchyItemData()
            {
                Id = x.Id,
                Name = x.Name,
                ChildrenItems = x.Children?.Select(c => new HierarchyItemData{Id = c.Id, Name = c.Name}).ToList()
            } as HierarchyItemData).ToList();
            _hierarchyView.Initialize(hierarchyData, hierarchyMenu, _uiCamera);
        }

        public void UpdateSelectedItem(Guid id)
        {
            _hierarchyView.UpdateSelectItem(id);
        }

        public void AddItem(HierarchyEntity entity, int index = 0)
        {
            var itemData = new HierarchyItemData()
            {
                Id = entity.Id,
                Name = entity.Name
            };
            _hierarchyView.AddItem(itemData, index);
        }

        public void DeleteItem(HierarchyEntity entity)
        {
            _hierarchyView.DeleteItem(entity.Id);
        }

        public void InsertItem(HierarchyEntity entity, int index)
        {
            var itemData = new HierarchyItemData()
            {
                Id = entity.Id,
                Name = entity.Name
            };
            StartCoroutine(_hierarchyView.InsertItem(itemData, null, index));
        }

        public void RenameFixture(Guid id, string name)
        {
            _hierarchyView.RenameItem(id, name);
        }
    }


}
