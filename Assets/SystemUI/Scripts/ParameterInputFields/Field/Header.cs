using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace inc.stu.SystemUI
{
    
    [ExecuteInEditMode]
    public class Header : MonoBehaviour
    {
        [SerializeField] private string _headerTitle = "Header";
        [SerializeField] private Color _backgroundColor = Color.gray;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _backgroundImage;

        // Update is called once per frame
        private void Update()
        {
            if (_text != null)
            {
                _text.text = _headerTitle;
            }

            if (_backgroundImage != null)
            {
                _backgroundImage.color = _backgroundColor;
            }
        }
    }

}

