using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{

    public abstract class MinMaxSlider<T> : Field<T>
    {

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_InputField _minInputField;
        [SerializeField] private TMP_InputField _maxInputField;
        [SerializeField] private Slider _slider;
        
        [SerializeField] protected T _minValue;
        [SerializeField] protected T _maxValue;

        private readonly Subject<T> _onValueChanged = new();
        public override IObservable<T> OnValueChanged => _onValueChanged;

        private T _value;
        public override T Value => _value;

        protected override void Awake()
        {
            base.Awake();

            if (Application.isPlaying)
            {
                SetSliderMinMax(_minValue, _maxValue);
                
                _inputField.onEndEdit.AsObservable().Subscribe(x =>
                {
                    var value = Parse(x);
                    if (_value.Equals(value)) return;
                    
                    _onValueChanged.OnNext(value);
                }).AddTo(this);

                _slider.OnValueChangedAsObservable().Subscribe(x =>
                {
                    if (_value.Equals(x)) return;
                    
                    _onValueChanged.OnNext(CastSliderFloatToGenericValue(x));
                }).AddTo(this);
                
                // when value change notified, change ui data
                _onValueChanged.Subscribe(SetValueWithoutNotify).AddTo(this);

                _minInputField.onEndEdit.AsObservable().Subscribe(x =>
                {
                    SetSliderMinMax(Parse(x), _maxValue);
                }).AddTo(this);
                
                _maxInputField.onEndEdit.AsObservable().Subscribe(x =>
                {
                    SetSliderMinMax(_minValue, Parse(x));
                }).AddTo(this);
            }
            
        }
        
        public void SetSliderMinMax(T min, T max)
        {
            _minValue = min;
            _minInputField.SetTextWithoutNotify(_minValue.ToString());
            _maxValue = max;
            _maxInputField.SetTextWithoutNotify(_maxValue.ToString());
            
            SetMinMaxToSliderComponent(_slider);
        }

        protected abstract void SetMinMaxToSliderComponent(Slider sliderComponent);
        protected abstract T CastSliderFloatToGenericValue(float value);
        protected abstract float CastGenericValueForSliderFloat(T value);
        protected abstract T Parse(string value);
        
        public override void SetValueWithNotify(T value)
        {
            if (_value.Equals(value)) return;
            
            _inputField.SetTextWithoutNotify(value.ToString());
            _slider.SetValueWithoutNotify(CastGenericValueForSliderFloat(value));
            
            _value = value;
            _onValueChanged.OnNext(value);
        }

        public override void SetValueWithoutNotify(T value)
        {
            if (_value.Equals(value)) return;

            _inputField.SetTextWithoutNotify(value.ToString());
            _slider.SetValueWithoutNotify(CastGenericValueForSliderFloat(value));
            
            _value = value;
        }

        protected override void Update()
        {
            if (!_inputField) return;

            if (Interactable)
            {
                _inputField.image.color = new Color(0.1640625f, 0.1640625f, 0.1640625f);
                _inputField.textComponent.color = Color.white;
            }
            else
            {
                _inputField.image.color = Color.black;
                _inputField.textComponent.color = Color.cyan;
            }
            
            _inputField.interactable = Interactable;
            
        }
    }

}
