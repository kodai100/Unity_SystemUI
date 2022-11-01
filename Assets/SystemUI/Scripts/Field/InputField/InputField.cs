using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI
{
    public abstract class InputField<T> : Field<T>
    {
        
        [SerializeField] private TMP_InputField _inputField;

        private readonly Subject<T> _subject = new();
        private T _value;
        
        public override IObservable<T> OnValueChanged => _subject;  // TODO: Notify latest value on subscribe ?
        public override T Value => _value;
        
        protected override void Awake()
        {
            base.Awake();

            if (Application.isPlaying)
            {
                SetupOnEditEvent(_inputField.onEndEdit.AsObservable());
            }
            
        }

        private void SetupOnEditEvent(IObservable<string> onEndEditObservable)
        {
            onEndEditObservable.Subscribe(x =>
            {
                var value = Parse(x);
                
                if (!value.Equals(_value))
                {
                    _value = value;
                    _subject.OnNext(_value);
                }
               
            }).AddTo(this);
        }

        protected abstract T Parse(string value);
        protected abstract string ValueToText(T value);
        
        public override void SetValueWithNotify(T value)
        {
            if (_value.Equals(value)) return;
            
            _value = value;
            _inputField.SetTextWithoutNotify(ValueToText(value));
            _subject.OnNext(_value);
        }

        public override void SetValueWithoutNotify(T value)
        {
            _value = value;
            _inputField.SetTextWithoutNotify(ValueToText(value));
        }
        
        protected override void Update()
        {
            if (!_inputField) return;

            if (Interactable)
            {
                _inputField.image.color = new Color(0.1640625f, 0.1640625f, 0.1640625f);
                _inputField.textComponent.color = Color.white;
            }
            else
            {
                _inputField.image.color = Color.black;
                _inputField.textComponent.color = Color.cyan;
            }
            
            _inputField.interactable = Interactable;
            
        }
    }
}
