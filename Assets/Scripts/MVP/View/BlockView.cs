using MVP.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.View
{
    /// <summary>
    /// MonoBehaviour implementation of IBlockView.
    /// </summary>
    public sealed class BlockView : MonoBehaviour, IBlockView
    {
        [SerializeField] private Image _image;

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetImage(Sprite sprite)
        {
            if (_image != null)
            {
                _image.sprite = sprite;
            }
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}

