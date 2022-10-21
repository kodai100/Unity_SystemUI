using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    using UniRx;
    using UnityEngine;

    [ExecuteInEditMode]
    public class StringParameterUI : MonoBehaviour
    {
        [SerializeField] private string label = "Parameter";

        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_InputField inputField;

        private Color defaultBackgroundColor;
        private Subject<string> onUpdate = new Subject<string>();

        public IObservable<string> OnUpdateAsObservable => onUpdate;

        private string value = "";

        private void Start()
        {
            if (!Application.isPlaying) return;

            defaultBackgroundColor = backgroundImage.color;

            inputField.onValueChanged.AsObservable().Skip(1).Subscribe(value =>
            {
                onUpdate.OnNext(value);
                this.value = value;
                backgroundImage.color = defaultBackgroundColor;
            }).AddTo(this);
        }

        public void Set(string value)
        {
            inputField.text = value;
            // this.value = value;
        }

        public string Emit()
        {
            onUpdate.OnNext(this.value);
            return value;
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
