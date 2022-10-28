using System;
using UniRx;
using UnityEngine;


namespace inc.stu.SystemUI
{
    
    public class Vector2Field : Field<Vector2>
    {

        [SerializeField] private Field<float> _xField;
        [SerializeField] private Field<float> _yField;

        private Vector2 _value;
        
        public override IObservable<Vector2> OnValueChanged => _onValueChanged;
        private readonly Subject<Vector2> _onValueChanged = new();

        public override Vector2 Value => _value;

        protected override void Update()
        {
            base.Update();

            _xField.Interactable = Interactable;
            _yField.Interactable = Interactable;
        }

        public override void SetValueWithNotify(Vector2 value)
        {
            if (_value == value) return;
            _value = value;
            
            _xField.SetValueWithoutNotify(value.x);
            _yField.SetValueWithoutNotify(value.y);
            
            _onValueChanged.OnNext(value);
        }

        public override void SetValueWithoutNotify(Vector2 value)
        {
            if (_value == value) return;
            
            _value = value;
            
            _xField.SetValueWithoutNotify(value.x);
            _yField.SetValueWithoutNotify(value.y);
        }

        protected override void Awake()
        {
            
            base.Awake();

            _xField.OnValueChanged.Subscribe(x =>
            {
                var value = new Vector2(x, _value.y);
                SetValueWithNotify(value);
            }).AddTo(this);
            
            _yField.OnValueChanged.Subscribe(y =>
            {
                var value = new Vector2(_value.x, y);
                SetValueWithNotify(value);
            }).AddTo(this);

        }

    }

}

