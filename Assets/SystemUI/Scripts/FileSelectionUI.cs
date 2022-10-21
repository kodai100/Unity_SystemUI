using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.UIUtilities
{
    
    [ExecuteInEditMode]
    public class FileSelectionUI : MonoBehaviour
    {

        [SerializeField] private string _label = "File";
        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _button;

        [SerializeField] private string _defaultDirectoryPath = Application.streamingAssetsPath;
        [SerializeField] private string[] _extensions = new[] { "json" };

        public IObservable<string> OnValueChanged => _inputField.onValueChanged.AsObservable();
        public IObservable<Unit> OnClicked => _button.OnClickAsObservable();

        public string[] Extensions
        {
            get => _extensions;
            set => _extensions = value;
        }

        public string DefaultDirectoryPath
        {
            get => _defaultDirectoryPath;
            set => _defaultDirectoryPath = value;
        }
        
        private void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ =>
            {
                // var path = FileBrowser.Instance.OpenSingleFile("Open file", _defaultDirectoryPath, "", _extensions);
                // SetValueWithNotify(path);
            }).AddTo(this);
        }

        private void Update()
        {
            if (_labelText)
            {
                _labelText.text = _label;
            }
        }
        
        public void SetValueWithoutNotify(string path)
        {
            _inputField.SetTextWithoutNotify(path);
        }

        public void SetValueWithNotify(string path)
        {
            _inputField.text = path;
        }

    }

}

