using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    using UniRx;
    using UnityEngine;

    [ExecuteInEditMode]
    public class Vector3ParameterUI : MonoBehaviour
    {
        [SerializeField] private string label = "Parameter";
        [SerializeField] private bool isRealtimeUpdate = true;

        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_InputField inputField1;
        [SerializeField] private TMP_InputField inputField2;
        [SerializeField] private TMP_InputField inputField3;

        private Color defaultBackgroundColor;
        private Subject<Vector3> onUpdate = new Subject<Vector3>();

        public IObservable<Vector3> OnUpdateAsObservable => onUpdate;

        private Vector3 vector3Composite = Vector3.zero;

        private void Start()
        {
            if (!Application.isPlaying) return;

            defaultBackgroundColor = backgroundImage.color;

            inputField1.onValueChanged.AsObservable().Skip(1).Subscribe(value =>
            {
                if (float.TryParse(value, out var result))
                {
                    vector3Composite.x = result;
                    if (isRealtimeUpdate) onUpdate.OnNext(vector3Composite);
                    backgroundImage.color = defaultBackgroundColor;
                }
                else
                {
                    backgroundImage.color = Color.red;
                }
            }).AddTo(this);

            inputField2.onValueChanged.AsObservable().Skip(1).Subscribe(value =>
            {
                if (float.TryParse(value, out var result))
                {
                    vector3Composite.y = result;
                    if (isRealtimeUpdate) onUpdate.OnNext(vector3Composite);
                    backgroundImage.color = defaultBackgroundColor;
                }
                else
                {
                    backgroundImage.color = Color.red;
                }
            }).AddTo(this);

            inputField3.onValueChanged.AsObservable().Skip(1).Subscribe(value =>
            {
                if (float.TryParse(value, out var result))
                {
                    vector3Composite.z = result;
                    if (isRealtimeUpdate) onUpdate.OnNext(vector3Composite);
                    backgroundImage.color = defaultBackgroundColor;
                }
                else
                {
                    backgroundImage.color = Color.red;
                }
            }).AddTo(this);
        }

        public void Set(Vector3 value)
        {
            Debug.Log(value);
            inputField1.text = value.x.ToString("0.00");
            inputField2.text = value.x.ToString("0.00");
            inputField3.text = value.x.ToString("0.00");
            // this.value = value;
        }

        public Vector3 Emit()
        {
            onUpdate.OnNext(vector3Composite);
            return vector3Composite;
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
