using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI.MenuBar
{
    public class TestFileMenu : MenuBarBase
    {
        private readonly Subject<Unit> _clickNew = new();
        private readonly Subject<Unit> _clickLoad = new();
        private readonly Subject<Unit> _clickSave = new();
        private readonly Subject<Unit> _clickSaveAs = new();

        public IObservable<Unit> OnClickNew => _clickNew;
        public IObservable<Unit> OnClickLoad => _clickLoad;
        public IObservable<Unit> OnClickSave => _clickSave;
        public IObservable<Unit> OnClickSaveAs => _clickSaveAs;
        
        public TestFileMenu()
        {
            Name = "File";

            Items = new List<MenuBarItem>
            {
                new()
                {
                    Name = "New",
                    Action = () => _clickNew.OnNext(Unit.Default),
                },
                new()
                {
                    Name = "Load",
                    Action = () => _clickLoad.OnNext(Unit.Default),
                },
                new()
                {
                    Name = "Save",
                    Action = () => _clickSave.OnNext(Unit.Default),
                },
                new()
                {
                    Name = "Save As...",
                    Action = () => _clickSaveAs.OnNext(Unit.Default),
                }
            };
            
            
            ChildMenus.Add(new MenuBarBase()
            {
                Name = "Children",
                Items = new List<MenuBarItem>
                {
                    new(){Name = "Child1", Action = () => {Debug.Log("Child1");}},
                    new(){Name = "Child2", Action = () => {Debug.Log("Child2");}},
                },
                ChildMenus = new List<MenuBarBase>()
                {
                    new()
                    {
                        Name = "Inside",
                        Items = new List<MenuBarItem>
                        {
                            new(){Name = "Inside1", Action = () => {Debug.Log("Inside1");}},
                            new(){Name = "Inside2", Action = () => {Debug.Log("Inside2");}},
                        },
                        ChildMenus = new List<MenuBarBase>()
                        {
                            new()
                            {
                                Name = "Inside2",
                                Items = new List<MenuBarItem>
                                {
                                    new(){Name = "Inside1", Action = () => {Debug.Log("Inside1");}},
                                    new(){Name = "Inside2", Action = () => {Debug.Log("Inside2");}},
                                },
                            }
                        }
                    }
                }
            });
            
            // base.Initialize();
        }
    }

}

