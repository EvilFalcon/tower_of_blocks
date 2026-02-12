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

        private void Awake()
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
        }

        public void SetPosition(Vector2 position)
        {
            RectTransform rect = transform as RectTransform;

            if (rect != null)
            {
                rect.anchoredPosition = position;
            }
            else
            {
                transform.position = position;
            }
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