using UnityEngine;
using UnityEngine.UI;

namespace MVP.View
{
    [RequireComponent(typeof(ScrollRect))]
    public sealed class BlocksScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        public ScrollRect ScrollRect => _scrollRect != null ? _scrollRect : (_scrollRect = GetComponent<ScrollRect>());

        private void Awake()
        {
            if (_scrollRect == null)
                _scrollRect = GetComponent<ScrollRect>();
        }
    }
}

