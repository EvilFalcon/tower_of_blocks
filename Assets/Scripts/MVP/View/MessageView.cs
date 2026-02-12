using TowerOfBlocks.MVP.Interfaces;
using UnityEngine;
using TMPro;

namespace TowerOfBlocks.MVP.View
{
    /// <summary>
    /// MonoBehaviour implementation of IMessageView.
    /// </summary>
    public sealed class MessageView : MonoBehaviour, IMessageView
    {
        [SerializeField] private TMP_Text text;

        public void ShowMessage(string value)
        {
            if (text != null)
            {
                text.text = value;
            }

            gameObject.SetActive(true);
        }

        public void HideMessage()
        {
            gameObject.SetActive(false);
        }
    }
}

