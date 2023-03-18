using System;
using UniRx;
using UnityEngine;


namespace inc.stu.SystemUI
{
    
    public class Vector3Field : Field<Vector3>
    {

        [SerializeField] private Field<float> _xField;
        [SerializeField] private Field<float> _yField;
        [SerializeField] private Field<float> _zField;

        private Vector3 _value;
        
        public override IObservable<Vector3> OnValueChanged => _onValueChanged;
        private readonly Subject<Vector3> _onValueChanged = new();

        public override Vector3 Value => _value;

        protected override void Update()
        {
            base.Update();

            _xField.Interactable = Interactable;
            _yField.Interactable = Interactable;
            _zField.Interactable = Interactable;
        }

        public override void SetValueWithNotify(Vector3 value)
        {
            if (_value == value) return;
            _value = value;
            
            _xField.SetValueWithoutNotify(value.x);
            _yField.SetValueWithoutNotify(value.y);
            _zField.SetValueWithoutNotify(value.z);
            
            _onValueChanged.OnNext(value);
        }

        public override void SetValueWithoutNotify(Vector3 value)
        {
            if (_value == value) return;
            
            _value = value;
            
            _xField.SetValueWithoutNotify(value.x);
            _yField.SetValueWithoutNotify(value.y);
            _zField.SetValueWithoutNotify(value.z);
        }

        protected override void Awake()
        {
            
            base.Awake();

            _xField.OnValueChanged.Subscribe(x =>
            {
                var value = new Vector3(x, _value.y, _value.z);
                SetValueWithNotify(value);
            }).AddTo(this);
            
            _yField.OnValueChanged.Subscribe(y =>
            {
                var value = new Vector3(_value.x, y, _value.z);
                SetValueWithNotify(value);
            }).AddTo(this);
            
            _zField.OnValueChanged.Subscribe(z =>
            {
                var value = new Vector3(_value.x, _value.y, z);
                SetValueWithNotify(value);
            }).AddTo(this);


        }

    }

}

