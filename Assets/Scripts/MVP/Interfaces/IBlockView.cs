using UnityEngine;

namespace MVP.Interfaces
{
    /// <summary>
    /// View contract for a block.
    /// </summary>
    public interface IBlockView
    {
        void SetPosition(Vector2 position);
        void SetImage(Sprite sprite);
        void SetActive(bool isActive);
    }
}

