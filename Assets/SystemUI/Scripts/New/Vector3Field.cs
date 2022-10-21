using System;
using UniRx;
using UnityEngine;


namespace inc.stu.SystemUI
{
    public class Vector3Field : Field<Vector3>
    {

        [SerializeField] private float _x;
        [SerializeField] private float _y;
        [SerializeField] private float _z;

        public override IObservable<Vector3> OnValueChanged => _onValueChanged;
        private Subject<Vector3> _onValueChanged = new();

    }

}

