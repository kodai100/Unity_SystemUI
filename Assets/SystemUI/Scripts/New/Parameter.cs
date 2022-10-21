using System;
using TMPro;
using UnityEngine;

namespace inc.stu.SystemUI
{
    
    [ExecuteInEditMode]
    public abstract class Parameter<T> : MonoBehaviour
    {
        [SerializeField] private string _label = $"Parameter ({nameof(T)})";
        [SerializeField] private TMP_Text _labelText;
        
        public abstract Field<T> Field { get; }

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

