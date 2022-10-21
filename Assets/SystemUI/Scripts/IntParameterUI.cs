using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    using UniRx;
    using UnityEngine;

    [ExecuteInEditMode]
    public class IntParameterUI : MonoBehaviour
    {
        [SerializeField] private string _label = "Parameter";

        [SerializeField] private int _defaultValue;

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] private Color _errorColor = Color.red;
        
        private Color _defaultBackgroundColor;
        private readonly Subject<int> _onUpdate = new();

        public IObservable<int> OnUpdateAsObservable => _onUpdate;

        private void Start()
        {
            if (!Application.isPlaying) return;

            _defaultBackgroundColor = _backgroundImage.color;

            _inputField.onValueChanged.AsObservable().Subscribe(value =>
            {
                if (int.TryParse(value, out var result))
                {
                    _onUpdate.OnNext(result);
                    _backgroundImage.color = _defaultBackgroundColor;
                    SetValueWithNotify(result);
                }
                else
                {
                    _backgroundImage.color = _errorColor;
                }
            }).AddTo(this);
            
        }

        public int Value => int.Parse(_inputField.text);    // TODO: private変数にキャッシュ

        public void SetValueWithoutNotify(int value)
        {
            _inputField.SetTextWithoutNotify(value.ToString());
        }

        public void SetValueWithNotify(int value)
        {
            _inputField.text = value.ToString();
        }

        private void Update()
        {
            if (_labelText)
            {
                _labelText.text = _label;
            }
        }

        private void OnDestroy()
        {
            _onUpdate.Dispose();
        }
    }
}
