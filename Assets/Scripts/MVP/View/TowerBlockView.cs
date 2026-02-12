using MVP.Interfaces;
using UnityEngine;

namespace MVP.View
{
    /// <summary>
    /// MonoBehaviour implementation of ITowerBlockView.
    /// </summary>
    public sealed class TowerBlockView : MonoBehaviour, ITowerBlockView
    {
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}

