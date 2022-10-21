using System;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    using UniRx;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ButtonParameterUI : MonoBehaviour
    {
        [SerializeField] private string label = "Parameter";

        [SerializeField] private TMP_Text labelText;
        [SerializeField] private Button button;

        private Color defaultBackgroundColor;
        private Subject<Unit> onUpdate = new Subject<Unit>();

        public IObservable<Unit> OnUpdateAsObservable => onUpdate;

        private void Start()
        {
            if (!Application.isPlaying) return;

            button.OnClickAsObservable().Subscribe(_ =>
            {
                onUpdate.OnNext(Unit.Default);
            }).AddTo(this);
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
