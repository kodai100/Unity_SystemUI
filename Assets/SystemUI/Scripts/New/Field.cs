using System;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI
{
    public abstract class Field<T> : MonoBehaviour
    {
        public abstract IObservable<T> OnValueChanged { get; }
    }

}

