using System;
using TMPro;
using UnityEngine;

namespace inc.stu.SystemUI
{

    [ExecuteInEditMode]
    public abstract class Parameter<T> : MonoBehaviour
    {
        [SerializeField] private string _label = $"Parameter ({typeof(T).Name})";
        [SerializeField] private TMP_Text _labelText;
        
        [SerializeField] private Field<T> _field;
        public Field<T> Field => _field;

        public IObservable<T> OnValueChanged => Field.OnValueChanged;
        
        protected virtual void UpdateInternal(){}
        
        private void Update()
        {
            if (_labelText != null)
            {
                _labelText.text = _label;
            }
            UpdateInternal();
        }
    }
    
}

