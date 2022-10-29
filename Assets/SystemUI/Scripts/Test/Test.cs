using System;
using UniRx;
using UnityEngine;

namespace inc.stu.SystemUI
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Parameter<float> _fieldFloat;
        [SerializeField] private Parameter<int> _fieldInt;
        [SerializeField] private Parameter<Vector2> _fieldVector2;
        [SerializeField] private Parameter<Vector3> _fieldVector3;
        [SerializeField] private Parameter<string> _fieldString;
        [SerializeField] private Parameter<bool> _fieldBool;
        [SerializeField] private Parameter<string> _fieldFile;
        
        [SerializeField] private Parameter<float> _sliderFloat;
        [SerializeField] private Parameter<int> _sliderInt;

        [SerializeField] private Parameter<bool> _buttonOneShot;
        [SerializeField] private Parameter<bool> _buttonToggle;

        [SerializeField] private Console _console;

        private void Start()
        {
            
            Application.logMessageReceived += OnLogMessage;

            ParameterTest(_fieldFloat, 100);
            ParameterTest(_fieldInt, 1508);
            ParameterTest(_fieldVector2, Vector2.one);
            ParameterTest(_fieldVector3, Vector3.one);
            ParameterTest(_fieldString, "Hello!");
            ParameterTest(_fieldBool, true);
            ParameterTest(_fieldFile, "file://");
            
            ParameterTest(_sliderFloat, 0.5f);
            ParameterTest(_sliderInt, 4);
            
            ParameterTest(_buttonOneShot, true);
            ParameterTest(_buttonToggle, true);
            
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
        
        
        private void OnLogMessage( string logText, string stackTrace, LogType logType )
        {
            if( string.IsNullOrEmpty( logText ) )
            {
                return;
            }

            switch (logType)
            {
                case LogType.Log : 
                    _console.Log(logText); 
                    return;
                case LogType.Error:
                    _console.Error(logText);
                    return;
                case LogType.Assert:
                    _console.Log(logText);
                    return;
                case LogType.Exception:
                    _console.Error(logText);
                    return;
                case LogType.Warning:
                    _console.Warning(logText);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        } 


        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessage;
        }
    }

}

