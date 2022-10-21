using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    
    public class ToggleButtonUI : MonoBehaviour
    {

        [SerializeField] private Button _button;

        [SerializeField] private Image _image;
        [SerializeField] private Sprite _enabledSprite;
        [SerializeField] private Sprite _disabledSprite;

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color _enabledColor = Color.green;
        [SerializeField] private Color _disabledColor = Color.grey;

        private readonly Subject<bool> _onReceiveToggled = new();
        public IObservable<bool> OnToggleClicked => _onReceiveToggled;
        
        private bool _toggleState;
        
        private void Awake()
        {
            _button.OnClickAsObservable().Subscribe(_ =>
            {
                SetValueWithNotify(!_toggleState);

            }).AddTo(this);

            SetValueWithoutNotify(false);

        }

        public bool Value => _toggleState;
        
        public void SetValueWithNotify(bool isEnabled)
        {
            if (isEnabled == _toggleState) return;
            
            _image.sprite = GetImageSprite(isEnabled);
            _backgroundImage.color = GetBackgroundColor(isEnabled);
            _onReceiveToggled.OnNext(isEnabled);

            _toggleState = isEnabled;
        }

        public void SetValueWithoutNotify(bool isEnabled)
        {
            _toggleState = isEnabled;
            _image.sprite = GetImageSprite(isEnabled);
            _backgroundImage.color = GetBackgroundColor(isEnabled);
        }

        private Sprite GetImageSprite(bool isEnabled)
        {
            return isEnabled ? _enabledSprite : _disabledSprite;
        }

        private Color GetBackgroundColor(bool isEnabled)
        {
            return isEnabled ? _enabledColor : _disabledColor;
        }

        private void OnDestroy()
        {
            _onReceiveToggled.OnNext(false);
            _onReceiveToggled.Dispose();
        }

    }

}
