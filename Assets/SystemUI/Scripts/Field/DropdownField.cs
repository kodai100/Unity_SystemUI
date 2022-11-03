using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI
{

    public abstract class DropdownField<T> : Field<T>
    {

        [SerializeField] private TMP_Dropdown _dropdown;

        private readonly Subject<T> _subject = new();
        private T _value;

        protected abstract T ParseToEnum(int value);
        protected abstract int ParseToInt(T value);

        protected abstract List<TMP_Dropdown.OptionData> CreateOptions();
        
        public override T Value => _value;
        public override IObservable<T> OnValueChanged => _subject;


        protected override void Awake()
        {
            base.Awake();

            var option = CreateOptions();
            _dropdown.options = option;

            _dropdown.onValueChanged.AsObservable().Subscribe(value =>
            {
                var v = ParseToEnum(value);
                
                if (!value.Equals(_value))
                {
                    _value = ParseToEnum(value);
                    _subject.OnNext(v);
                }
            }).AddTo(this);
        }
        
        protected override void Update()
        {
            if (!_dropdown) return;
            
            _dropdown.interactable = Interactable;
        }
        
        public override void SetValueWithNotify(T value)
        {
            if (_value.Equals(value)) return;
            
            _value = value;
            _dropdown.SetValueWithoutNotify(ParseToInt(_value));
            _subject.OnNext(value);
            
        }

        public override void SetValueWithoutNotify(T value)
        {
            _value = value;
            _dropdown.SetValueWithoutNotify(ParseToInt(_value));
        }

    }

}