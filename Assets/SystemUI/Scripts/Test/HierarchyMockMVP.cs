using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Previz.Hierarchy;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI.Tests
{
    public class HierarchyMockMVP : MonoBehaviour
    {

        [SerializeField] private TestHierarchyView _testHierarchyView;
        
        private HierarchyPresenter _presenter;

        private HierarchyModel _model;
        
        private List<IDisposable> _disposables = new();
        
        // Start is called before the first frame update
        private void Start()
        {
            _model = new HierarchyModel();

            _presenter = new HierarchyPresenter(_testHierarchyView);
            _presenter.Setup(_model.HierarchyEntityList.Value.ToArray());

            // _presenter.OnItemDroppedIn.Subscribe(x => _model.).AddTo(_disposables);
            _presenter.OnItemInserted.Subscribe(x => _model.InsertEntity(x.MoveItemId, x.AboveItemId)).AddTo(_disposables);
            _presenter.OnClickEntity.Subscribe(x => _model.UpdateSelectedEntity(x)).AddTo(_disposables);;
            _presenter.OnDeleteEntity.Subscribe(_model.RemoveEntity).AddTo(_disposables);;
            _presenter.OnCreateEntity.Subscribe(_model.AddEntity).AddTo(_disposables);;



            _model.OnSelectedEntityChanged.SkipLatestValueOnSubscribe().Subscribe(x => _presenter.UpdateSelectedEntity(x)).AddTo(_disposables);;
            _model.OnAddEntity.Subscribe(x =>
            {
                var index = _model.HierarchyEntityList.Value.Count(_ => true);
                _presenter.AddEntity(x, index);
            }).AddTo(_disposables);;
            _model.OnDeleteEntity.Subscribe(x => _presenter.RemoveEntity(x)).AddTo(_disposables);;
            _model.OnInsertEntity.Subscribe(x => _presenter.InsertEntity(x.entity, x.parentId, x.index)).AddTo(_disposables);

        }


        private void OnDestroy()
        {
            _disposables.ForEach(d =>
            {
                d.Dispose();
            });
        }
    }


    public class HierarchyEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<HierarchyEntity> Children { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class HierarchyPresenter
    {
        private TestHierarchyView _testHierarchyView;
        
        private TestHierarchyMenu _hierarchyMenu = new();
        
        public IObservable<HierarchyItemDropInEvent> OnItemDroppedIn => _testHierarchyView.OnItemDroppedIn;
        public IObservable<HierarchyItemInsertEvent> OnItemInserted => _testHierarchyView.OnItemInserted;
        public IObservable<Guid> OnClickEntity => _testHierarchyView.OnSelectedItem.Select(x => x.Id);
        
        public IObservable<string> OnCreateEntity => _hierarchyMenu.OnClickAddName;
        public IObservable<Guid> OnDeleteEntity => _hierarchyMenu.OnClickDeleteItem.Select(x => x.Id);

        public HierarchyPresenter(TestHierarchyView testHierarchyView)
        {
            _testHierarchyView = testHierarchyView;
        }
        
        public void Setup(HierarchyEntity[] fixtureEntities)
        {
            _testHierarchyView.Setup(fixtureEntities, _hierarchyMenu);
        }

        public void UpdateSelectedEntity(HierarchyEntity entity)
        {
            Debug.Log($"{entity.Id} : {entity.Name}");
            _testHierarchyView.UpdateSelectedItem(entity.Id);
        }

        public void AddEntity(HierarchyEntity entity, int index)
        {
            _testHierarchyView.AddItem(entity, index);
        }
        public void RemoveEntity(HierarchyEntity entity)
        {
            _testHierarchyView.DeleteItem(entity);
        }
        public void InsertEntity(HierarchyEntity entity, Guid? parentId, int index)
        {
            _testHierarchyView.InsertItem(entity, parentId, index);
        }

    }
    

    public class HierarchyModel
    {

        private ReactiveProperty<List<HierarchyEntity>> _hierarchyEntityList = new();
        public IReadOnlyReactiveProperty<List<HierarchyEntity>> HierarchyEntityList => _hierarchyEntityList;
        private ReactiveProperty<HierarchyEntity> _selectedEntity = new();
        public IReadOnlyReactiveProperty<HierarchyEntity> OnSelectedEntityChanged => _selectedEntity;
        
        private Subject<(HierarchyEntity entity, Guid? parentId, int index)> _onInsertEntity = new();
        public IObservable<(HierarchyEntity entity, Guid? parentId, int index)> OnInsertEntity => _onInsertEntity;

        private Subject<HierarchyEntity> _onDeleteEntity = new();
        public IObservable<HierarchyEntity> OnDeleteEntity => _onDeleteEntity;
        
        private Subject<HierarchyEntity> _onAddEntity = new();
        public IObservable<HierarchyEntity> OnAddEntity => _onAddEntity;

        public HierarchyModel()
        {
            // Init test data
            _hierarchyEntityList.Value = new List<HierarchyEntity>
            {
                new() { 
                    Id = Guid.NewGuid(),
                    Name = "Alice", 
                    Children = new List<HierarchyEntity>
                    {
                        new(){Id = Guid.NewGuid(), Name = "Baby"},
                        new(){Id = Guid.NewGuid(), Name = "Baby2"}
                    }},
                new() { Id = Guid.NewGuid(), Name = "Bob" },
                new() { Id = Guid.NewGuid(), Name = "Charlie" },
                new() { Id = Guid.NewGuid(), Name = "David" },
                new() { Id = Guid.NewGuid(), Name = "Eve" },
                new() { Id = Guid.NewGuid(), Name = "Frank" },
            };
            
        }

        public void UpdateSelectedEntity(Guid? id)
        {
            _selectedEntity.Value = id == null ? null : GetEntityRecursive(id.Value, null, _hierarchyEntityList.Value).entity;
        }
        
        private (HierarchyEntity entity, HierarchyEntity parentEntity, int index) GetEntityRecursive(Guid id, HierarchyEntity parentEntity, List<HierarchyEntity> entities)
        {
            var i = 0;
            foreach (var entity in entities)
            {
                if (entity.Id == id)
                {
                    return (entity, parentEntity, i);
                }
                if (entity.Children != null)
                {
                    var result = GetEntityRecursive(id, entity, entity.Children);
                    if (result.entity != null)
                    {
                        return result;
                    }
                }

                i++;
            }
            return (null, null, -1);
        }
        
        public void RemoveEntity(Guid id)
        {
            var targetEntity = _hierarchyEntityList.Value.SingleOrDefault(x => x.Id == id);
            if (targetEntity == null)
            {
                Debug.LogError($"ID : {id} のEntityがありません。");
                return;
            }
            _hierarchyEntityList.Value.RemoveAll(x => x.Id == id);
            _hierarchyEntityList.SetValueAndForceNotify(_hierarchyEntityList.Value);
            _onDeleteEntity.OnNext(targetEntity);
        }
        
        public void AddEntity(string name)
        {
            var newFixtureEntity = new HierarchyEntity()
            {
                Name = name,
                Id = Guid.NewGuid(),
            };
            _hierarchyEntityList.Value.Add(newFixtureEntity);
            
            _hierarchyEntityList.SetValueAndForceNotify(_hierarchyEntityList.Value);
            _onAddEntity.OnNext(newFixtureEntity);
            
            // AddFixtureを統合。おそらく追加した最新のFixtureを選択状態にする想定のコメントを見たので、ここでSelectedを変更
            _selectedEntity.Value = newFixtureEntity;
        }

        // public void DropInEntity(Guid itemId, Guid parentId, int index)
        // {
        //     var targetEntity = GetEntityRecursive(itemId, null, _hierarchyEntityList.Value);
        //     
        //     if(targetEntity.entity == null) return;
        //
        //     var targetParentEntity = GetEntityRecursive(parentId, null, _hierarchyEntityList.Value);
        //
        //     targetParentEntity.entity?.Children.Insert(0, targetEntity.entity);
        // }

        public void InsertEntity(Guid targetId, Guid? aboveItemId)
        {
            var targetEntity = GetEntityRecursive(targetId, null, _hierarchyEntityList.Value);
            
            if (targetEntity.entity == null)
            {
                Debug.LogError($"ID : {targetId} のEntityがありません。");
                return;
            }

            if (targetEntity.parentEntity != null)  // TODO: 要精査
            {
                // 動かしたアイテムを削除する
                targetEntity.parentEntity.Children.RemoveAll(x => x.Id == targetId);
            }

            // 挿入する場所の上のアイテムが無い場合、一番先頭に挿入する
            if (aboveItemId == null)
            {
                targetEntity.parentEntity.Children.Insert(0, targetEntity.entity);
                _hierarchyEntityList.SetValueAndForceNotify(_hierarchyEntityList.Value);
                _onInsertEntity.OnNext((targetEntity.entity, null, 0));
                return;
            }
            
            var aboveEntity =  GetEntityRecursive(aboveItemId.Value, null, _hierarchyEntityList.Value);
            if (aboveEntity.entity == null)
            {
                Debug.LogError($"ID : {aboveItemId} のFixtureEntityがありません。");
                return;
            }
            
            Debug.Log($"above : {aboveEntity.entity.Id}");

            // 上がトップノードの場合
            if (aboveEntity.parentEntity == null)
            {
                _hierarchyEntityList.Value.Insert(aboveEntity.index + 1, targetEntity.entity);
            }
            else
            {
                aboveEntity.parentEntity.Children.Insert(aboveEntity.index + 1, targetEntity.entity);
            }

            _hierarchyEntityList.SetValueAndForceNotify(_hierarchyEntityList.Value);
            _onInsertEntity.OnNext((targetEntity.entity, aboveEntity.parentEntity?.Id, aboveEntity.index + 1));
        }
        
    }
    
    
}

