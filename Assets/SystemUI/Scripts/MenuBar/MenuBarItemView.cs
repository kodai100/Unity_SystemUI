using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace inc.stu.SystemUI.MenuBar
{
    public class MenuBarItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _popupMenuRoot;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _deactiveColor;

        public IObservable<Unit> OnClick => _button.OnClickAsObservable();
        public IObservable<PointerEventData> OnMouseEnter => _button.OnPointerEnterAsObservable();
        public RectTransform PopupMenuRootRect => _popupMenuRoot;

        public void Setup(string name)
        {
            _text.text = name;
        }

        public void Activate()
        {
            _button.image.color = _activeColor;
        }

        public void Deactivate()
        {
            _button.image.color = _deactiveColor;
        }
    }
}