using MVP.Interfaces;
using TMPro;
using UnityEngine;

namespace MVP.View
{
    public sealed class MessageView : MonoBehaviour, IMessageView
    {
        [SerializeField] private TMP_Text _text;

        public void ShowMessage(string value)
        {
            if (_text != null)
            {
                _text.text = value;
            }

            gameObject.SetActive(true);
        }

        public void HideMessage()
        {
            gameObject.SetActive(false);
        }
    }
}

