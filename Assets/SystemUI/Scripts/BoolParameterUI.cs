using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    using UniRx;
    using UnityEngine;

    [ExecuteInEditMode]
    public class BoolParameterUI : MonoBehaviour
    {
        [SerializeField] private string label = "Parameter";

        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Toggle toggle;

        private Color defaultBackgroundColor;
        private Subject<bool> onUpdate = new Subject<bool>();

        public IObservable<bool> OnUpdateAsObservable => onUpdate;

        private void Start()
        {
            if (!Application.isPlaying) return;

            toggle.OnValueChangedAsObservable().Skip(1).Subscribe(isOn =>
            {
                onUpdate.OnNext(isOn);
            }).AddTo(this);
        }

        public void Set(bool isOn)
        {
            toggle.isOn = isOn;
        }

        private void Update()
        {
            if (labelText)
            {
                labelText.text = label;
            }
        }

        private void OnDestroy()
        {
            onUpdate.Dispose();
        }
    }
}
