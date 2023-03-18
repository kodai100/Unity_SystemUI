using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace inc.stu.SystemUI.MenuBar
{
    public class PopupMenuItemView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        /// <summary>
        /// 実際に表示されるTextオブジェクト
        /// </summary>
        [SerializeField] private TextMeshProUGUI _displayText;

        [SerializeField] private Image _hasChildMenuImage;

        public IObservable<Unit> OnClick => _button.OnClickAsObservable();
        public IObservable<PointerEventData> OnPointerEnter => _button.OnPointerEnterAsObservable();
        public IObservable<PointerEventData> OnPointerExit => _button.OnPointerExitAsObservable();

        public void Setup(string itemName, bool hasChild = false)
        {
            _displayText.text = itemName;
            
            _button.OnPointerEnterAsObservable().Subscribe(_ =>
            {
                _image.gameObject.SetActive(true);
            }).AddTo(this);
            _button.OnPointerExitAsObservable().Subscribe(_ =>
            {
                _image.gameObject.SetActive(false);
            }).AddTo(this);

            if (hasChild)
            {
                _hasChildMenuImage.gameObject.SetActive(true);
            }
            else
            {
                _hasChildMenuImage.gameObject.SetActive(false);
            }
        }
    }
}