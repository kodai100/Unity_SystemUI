using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class FileField : Field<string>
    {

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _button;
        
        private readonly Subject<string> _onValueChanged = new();

        private string _value;
        public override string Value => _value;
        public override IObservable<string> OnValueChanged => _onValueChanged;

        protected override void Awake()
        {
            base.Awake();

            _button.OnClickAsObservable().Subscribe(_ =>
            {
                _onValueChanged.OnNext("");
            }).AddTo(this);
        }
        
        public override void SetValueWithNotify(string value)
        {
            _value = value;
            _inputField.SetTextWithoutNotify(value);
            _onValueChanged.OnNext(value);
        }

        public override void SetValueWithoutNotify(string value)
        {
            _value = value;
            _inputField.SetTextWithoutNotify(value);
        }
    }

}

