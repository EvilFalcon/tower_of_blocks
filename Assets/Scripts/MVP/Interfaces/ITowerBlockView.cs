using UnityEngine;

namespace MVP.Interfaces
{
    /// <summary>
    /// View contract for a block in the tower.
    /// </summary>
    public interface ITowerBlockView
    {
        void SetPosition(Vector2 position);
        void SetActive(bool isActive);
    }
}

