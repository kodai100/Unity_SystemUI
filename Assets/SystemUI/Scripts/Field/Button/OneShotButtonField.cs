using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class OneShotButtonField : Field<bool>
    {

        [SerializeField] private Button _button;
        
        public override bool Value => true;

        public override IObservable<bool> OnValueChanged => _button.OnClickAsObservable().Select(x => true);

        public override void SetValueWithNotify(bool value)
        {
            // No
        }

        public override void SetValueWithoutNotify(bool value)
        {
            // No
            _button.onClick.Invoke();
        }
    }

}

