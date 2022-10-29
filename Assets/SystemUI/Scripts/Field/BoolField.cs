using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class BoolField : Field<bool>
    {

        [SerializeField] private Toggle _toggle;

        private bool _value;
        private readonly Subject<bool> _onValueChanged = new();

        public override bool Value => _value;

        public override IObservable<bool> OnValueChanged => _onValueChanged;

        protected override void Awake()
        {
            base.Awake();
            
            _value = false;
            _toggle.SetIsOnWithoutNotify(false);

            _toggle.OnValueChangedAsObservable().Subscribe(isOn =>
            {
                _value = isOn;
                _onValueChanged.OnNext(isOn);
            }).AddTo(this);
        }
        
        public override void SetValueWithNotify(bool value)
        {
            if (_value == value) return;
            _value = value;
            _toggle.SetIsOnWithoutNotify(value);
            _onValueChanged.OnNext(_value);
        }

        public override void SetValueWithoutNotify(bool value)
        {
            if (_value == value) return;
            _value = value;
            _toggle.SetIsOnWithoutNotify(value);
        }
    }

}

