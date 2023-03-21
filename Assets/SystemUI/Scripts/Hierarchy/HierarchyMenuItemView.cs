using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Previz.Hierarchy
{
    public class HierarchyMenuItemView : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _button;
        /// <summary>
        /// Prefab自体の大きさ調整のためのTextオブジェクト, このオブジェクトのテキストは表示されない
        /// </summary>
        [SerializeField] private TextMeshProUGUI _rootText;
        /// <summary>
        /// 実際に表示されるTextオブジェクト
        /// </summary>
        [SerializeField] private TextMeshProUGUI _displayText;

        public IObservable<Unit> OnClick => _button.OnClickAsObservable();
        public IObservable<PointerEventData> OnPointerEnter => _button.OnPointerEnterAsObservable();
        public IObservable<PointerEventData> OnPointerExit => _button.OnPointerExitAsObservable();

        public bool IsMouseInside { get; private set; }
        
        public void Setup(string itemName)
        {
            _rootText.text = itemName;
            _displayText.text = itemName;
            
            _button.OnPointerEnterAsObservable().Subscribe(_ =>
            {
                IsMouseInside = true;
                _backgroundImage.gameObject.SetActive(true);
            }).AddTo(this);
            _button.OnPointerExitAsObservable().Subscribe(_ =>
            {
                IsMouseInside = false;
                _backgroundImage.gameObject.SetActive(false);
            }).AddTo(this);
        }
    }
}