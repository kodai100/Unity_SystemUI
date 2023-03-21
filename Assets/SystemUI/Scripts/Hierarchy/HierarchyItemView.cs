using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Previz.Hierarchy
{
    public class HierarchyItemView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _toggleButton;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _toggleIconImage;
        [SerializeField] private RectTransform _indentObject;
        [SerializeField] private RectTransform _childrenRootRect;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _deactiveColor;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private Image _underAreaImage;

        public RectTransform ChildrenRootRect => _childrenRootRect;
        public bool IsShowChildren;
        public Guid ItemId;
        public int Depth;

        private bool _isDragging;
        private const int IndentUnitWidth = 35;
        private HierarchyItemData _data;

        public IObservable<HierarchyItemData> OnLeftClick => _text.OnPointerDownAsObservable()
            .Where(x => x.button == PointerEventData.InputButton.Left).Select(_ => _data);

        public IObservable<HierarchyItemData> OnRightClick => _text.OnPointerClickAsObservable()
            .Where(x => x.button == PointerEventData.InputButton.Right).Select(_ => _data);

        public IObservable<HierarchyItemData> OnBeginDrag => _button.OnBeginDragAsObservable().Select(_ => _data);
        public IObservable<HierarchyItemData> OnEndDrag => _button.OnEndDragAsObservable().Select(_ => _data);
        public IObservable<Unit> OnClickToggleIcon => _toggleButton.OnClickAsObservable();

        public IObservable<Unit> OnEnterUnderArea =>
            _underAreaImage.OnPointerEnterAsObservable().Select(_ => Unit.Default);

        public IObservable<Unit> OnExitUnderArea =>
            _underAreaImage.OnPointerExitAsObservable().Select(_ => Unit.Default);

        public IObservable<Unit> OnEnterTextArea => _button.OnPointerEnterAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnExitTextArea => _button.OnPointerExitAsObservable().Select(_ => Unit.Default);

        public void Setup(HierarchyItemData data, int depth)
        {
            _data = data;
            ItemId = data.Id;
            _text.text = data.Name;
            _indentObject.sizeDelta = new Vector2(depth * IndentUnitWidth, _indentObject.sizeDelta.y);
            Depth = depth;

            _underAreaImage.OnPointerExitAsObservable()
                .Where(_ => Math.Abs(_underAreaImage.color.a - 1) < 0.001)
                .Subscribe(_ =>
                {
                    var color = _underAreaImage.color;
                    color.a = 0;
                    _underAreaImage.color = color;
                }).AddTo(this);
        }

        public void RenameItem(string itemName)
        {
            _text.text = itemName;
        }

        public void Select()
        {
            _backgroundImage.color = _activeColor;
        }

        public void UnSelect()
        {
            _backgroundImage.color = _deactiveColor;
        }

        public void HideChildren()
        {
            var rect = _toggleIconImage.GetComponent<RectTransform>();
            rect.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            _childrenRootRect.gameObject.SetActive(false);
            IsShowChildren = false;
        }

        public void ShowChildren()
        {
            _childrenRootRect.gameObject.SetActive(true);
            var childViews = GetComponentsInChildren<HierarchyItemView>();
            foreach (var child in childViews)
            {
                if (child == this) continue;
                child.HideChildren();
            }

            var rect = _toggleIconImage.GetComponent<RectTransform>();
            rect.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            IsShowChildren = true;
        }

        public void ShowUnderAreaImage()
        {
            var color = _underAreaImage.color;
            color.a = 1;
            _underAreaImage.color = color;
        }

        public void HideUnderAreaImage()
        {
            var color = _underAreaImage.color;
            color.a = 0;
            _underAreaImage.color = color;
        }

        public void ShowHoverImage()
        {
            _backgroundImage.color = _hoverColor;
        }

        public void HideHoverImage()
        {
            _backgroundImage.color = _deactiveColor;
        }
    }
}