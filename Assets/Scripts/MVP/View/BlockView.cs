using TowerOfBlocks.MVP.Interfaces;
using UnityEngine;

namespace TowerOfBlocks.MVP.View
{
    /// <summary>
    /// MonoBehaviour implementation of IBlockView.
    /// </summary>
    public sealed class BlockView : MonoBehaviour, IBlockView
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetImage(Sprite sprite)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
            }
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}

