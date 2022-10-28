using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    public class Console : MonoBehaviour
    {

        [SerializeField] private ConsoleLine _consoleLinePrefab;

        [SerializeField] private ScrollRect _scrollView;
        [SerializeField] private RectTransform _container;

        [SerializeField] private Button _clearButton;

        private void Awake()
        {
            _clearButton.OnClickAsObservable().Subscribe(_ =>
            {
                Clear();
            }).AddTo(this);
        }

        public void Clear()
        {
            foreach (Transform child in _container)
            {
                Destroy(child.gameObject);
            }
        }
        
        public void Log(string message)
        {
            var line = Instantiate(_consoleLinePrefab, _container);
            line.Log(message);

            _scrollView.verticalNormalizedPosition = 0;
        }
        
        public void Warning(string message)
        {
            var line = Instantiate(_consoleLinePrefab, _container);
            line.Warning(message);
            
            _scrollView.verticalNormalizedPosition = 0;
        }
        
        public void Error(string message)
        {
            var line = Instantiate(_consoleLinePrefab, _container);
            line.Error(message);
            
            _scrollView.verticalNormalizedPosition = 0;
        }
    }

}

