using UnityEngine;
using UnityEngine.UI;

namespace MVP.View
{
    /// <summary>
    /// Wrapper over horizontal ScrollRect for the bottom blocks strip.
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public sealed class BlocksScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        public ScrollRect ScrollRect => _scrollRect;

        private void Awake()
        {
            if (_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }
        }
    }
}

