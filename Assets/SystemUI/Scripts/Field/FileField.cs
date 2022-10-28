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
        
        private readonly ReactiveProperty<string> _value = new();
        public override string Value => _value.Value;
        public override IObservable<string> OnValueChanged => _value;

        public IObservable<Unit> OnClickedAsObservable => _button.OnClickAsObservable();

        public override void SetValueWithNotify(string value)
        {
            _inputField.text = value;
        }

        public override void SetValueWithoutNotify(string value)
        {
            _inputField.SetTextWithoutNotify(value);
        }
    }

}

