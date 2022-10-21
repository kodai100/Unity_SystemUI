using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    using UniRx;
    using UnityEngine;

    [ExecuteInEditMode]
    public class DoubleParameterUI : MonoBehaviour
    {
        [SerializeField] private string label = "Parameter";

        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_InputField inputField;

        private Color defaultBackgroundColor;
        private Subject<double> onUpdate = new Subject<double>();

        public IObservable<double> OnUpdateAsObservable => onUpdate;

        private double value = 0;

        private void Start()
        {
            if (!Application.isPlaying) return;

            defaultBackgroundColor = backgroundImage.color;

            inputField.onValueChanged.AsObservable().Skip(1).Subscribe(value =>
            {
                if (double.TryParse(value, out var result))
                {
                    onUpdate.OnNext(result);
                    this.value = result;
                    backgroundImage.color = defaultBackgroundColor;
                }
                else
                {
                    backgroundImage.color = Color.red;
                }
            }).AddTo(this);
        }

        public void Set(double value)
        {
            inputField.text = value.ToString();
            // this.value = value;
        }

        public double Emit()
        {
            onUpdate.OnNext(this.value);
            return this.value;
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
