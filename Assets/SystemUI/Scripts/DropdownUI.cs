using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UniRx;

namespace inc.stu.UIUtilities
{
    

    [ExecuteInEditMode]
    public class DropdownUI : MonoBehaviour
    {
        [SerializeField] private string _label = "Parameter";

        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_Dropdown _dropdown;

        private Color _defaultBackgroundColor;

        public IObservable<int> OnUpdateAsObservable => _dropdown.onValueChanged.AsObservable();

        public int Value => _dropdown.value;
        
        public void SetValueWithNotify(int value)
        {
            _dropdown.value = value;
        }

        public void SetValueWithoutNotify(int value)
        {
            _dropdown.SetValueWithoutNotify(value);
        }

        public void SetupDropdownOptions(IEnumerable<string> options)
        {
            var optionData = options.Select(x => new TMP_Dropdown.OptionData(x));
            _dropdown.options = optionData.ToList();
        }
        
        private void Update()
        {
            if (_labelText)
            {
                _labelText.text = _label;
            }
        }
        
    }
}
