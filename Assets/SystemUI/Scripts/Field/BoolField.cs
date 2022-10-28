using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class BoolField : Field<bool>
    {

        [SerializeField] private Toggle _toggle;
        
        private readonly ReactiveProperty<bool> _value = new();

        public override bool Value => _value.Value;

        public override IObservable<bool> OnValueChanged => _value;
        
        public override void SetValueWithNotify(bool value)
        {
            _toggle.isOn = value;
        }

        public override void SetValueWithoutNotify(bool value)
        {
            _toggle.SetIsOnWithoutNotify(value);
        }
    }

}

