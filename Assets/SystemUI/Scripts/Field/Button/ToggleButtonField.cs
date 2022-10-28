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
            _button.image.color = value ? Color.black : new Color(0.1640625f, 0.1640625f, 0.1640625f);
            _isOn = value;
            _subject.OnNext(_isOn);
        }

        public override void SetValueWithoutNotify(bool value)
        {
            _button.image.color = value ? Color.black : new Color(0.1640625f, 0.1640625f, 0.1640625f);
            _isOn = value;
        }
    }

}

