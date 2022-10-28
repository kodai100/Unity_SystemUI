using TMPro;
using UnityEngine;

namespace inc.stu.SystemUI
{
    public class ConsoleLine : MonoBehaviour
    {

        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _errorColor = Color.red;

        [SerializeField] private TMP_Text _text;
        
        public void Error(string message)
        {
            _text.text = message;
            _text.color = _errorColor;
        }
        
        public void Warning(string message)
        {
            _text.text = message;
            _text.color = _warningColor;
        }
        
        public void Log(string message)
        {
            _text.text = message;
            _text.color = _normalColor;
        }
        
    }

}

