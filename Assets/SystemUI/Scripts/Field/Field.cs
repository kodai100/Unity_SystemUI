using System;
using UnityEngine;

namespace inc.stu.SystemUI
{
    
    [ExecuteInEditMode]
    public abstract class Field<T> : MonoBehaviour
    {

        [SerializeField] public bool Interactable = true;
        
        public abstract T Value { get; }
        public abstract IObservable<T> OnValueChanged { get; }
        public abstract void SetValueWithNotify(T value);
        public abstract void SetValueWithoutNotify(T value);

        protected virtual void Awake()
        {
        }

        protected virtual void Update()
        {
        }
    }

}

