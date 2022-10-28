using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Parameter<float> _fieldFloat;
        [SerializeField] private Parameter<int> _fieldInt;
        [SerializeField] private Parameter<Vector3> _fieldVector3;
        [SerializeField] private Parameter<string> _fieldString;
        [SerializeField] private Parameter<bool> _fieldBool;
        [SerializeField] private Parameter<string> _fieldFile;
        
        [SerializeField] private Parameter<float> _sliderFloat;
        [SerializeField] private Parameter<int> _sliderInt;

        [SerializeField] private Parameter<bool> _buttonOneShot;
        [SerializeField] private Parameter<bool> _buttonToggle;


        private void Start()
        {

            // ParameterTest(_fieldFloat, 100);
            // ParameterTest(_fieldInt, 1508);
            // ParameterTest(_fieldVector3, Vector3.one);
            // ParameterTest(_fieldString, "Hello!");
            // ParameterTest(_fieldBool, false);
            // ParameterTest(_fieldFile, "file://");
            
            ParameterTest(_sliderFloat, 0.5f);
            ParameterTest(_sliderInt, 4);
            
            // ParameterTest(_buttonOneShot, true);
            // ParameterTest(_buttonToggle, true);
            
        }

        private void ParameterTest<T>(Parameter<T> parameter, T initialValue)
        {
            // First, register on value changed event
            parameter.OnValueChanged.Subscribe(value =>
            {
                Debug.Log($"{typeof(T).Name} - Event : {value}");
            }).AddTo(this);
            
            // and after registration, initialize value with notification.
            // the UI will not emit event initially, so we have to call it ourselves.
            parameter.Field.SetValueWithNotify(initialValue);
            
            // we can get value directly
            Debug.Log($"{typeof(T).Name} - Direct : {parameter.Field.Value}");
        }
    }

}

