using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class ToggleButtonField : Field<bool>
    {

        [SerializeField] private Button _button;

        [SerializeField] private bool _isOn;

        [SerializeField] private Color _disabledColor = Color.gray;
        [SerializeField] private Color _enabledColor = Color.black;
        
        private readonly Subject<bool> _subject = new();

        public override bool Value => _isOn;

        public override IObservable<bool> OnValueChanged => _subject;
        
        protected override void Awake()
        {
            base.Awake();

            if (Application.isPlaying)
            {
                SetValueWithoutNotify(_isOn);
            
                _button.OnClickAsObservable().Subscribe(_ =>
                {
                    SetValueWithNotify(!_isOn);
                }).AddTo(this);
            }
            
        }
        
        public override void SetValueWithNotify(bool value)
        {
            if (_isOn == value) return;
            _button.image.color = value ? _enabledColor : _disabledColor;
            _isOn = value;
            _subject.OnNext(_isOn);
        }

        public override void SetValueWithoutNotify(bool value)
        {
            _button.image.color = value ? _enabledColor : _disabledColor;
            _isOn = value;
        }
    }

}

