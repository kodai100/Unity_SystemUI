using System;
using System.Collections.Generic;
using UniRx;

namespace Previz.Hierarchy
{
    public class TestHierarchyMenu : HierarchyMenuBase
    {
        private readonly Subject<HierarchyItemData> _clickAddSubject = new();
        private readonly Subject<HierarchyItemData> _clickDeleteSubject = new();
        private readonly Subject<string> _onClickAddNameSubject = new();

        public IObservable<HierarchyItemData> OnClickAddItem => _clickAddSubject;
        public IObservable<HierarchyItemData> OnClickDeleteItem => _clickDeleteSubject;
        public IObservable<string> OnClickAddName => _onClickAddNameSubject;


        private List<string> _testData = new()
        {
            "Alice", "Bob", "Chris", "David", "Eve", "Frank"
        };
        
        
        public TestHierarchyMenu()
        {
            Name = "";
            
            Items.Add(new HierarchyMenuItem()
            {
                Name = "Delete",
                Action = () => _clickDeleteSubject.OnNext(_currentItemData)
            });
            ChildMenu.Add(CreateAddEntityMenu());
        }

        private HierarchyMenuBase CreateAddEntityMenu()
        {
            List<HierarchyMenuItem> items = new();
            
            foreach (var data in _testData)
            {
                items.Add(new HierarchyMenuItem()
                {
                    Name = data,
                    Action = () => _onClickAddNameSubject.OnNext(data)
                });
            }
            
            return new HierarchyMenuBase()
            {
                Name = "Add",
                Items = items,
            };
        }
    }
}

