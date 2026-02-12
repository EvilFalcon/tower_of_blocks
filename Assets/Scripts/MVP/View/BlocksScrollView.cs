using UnityEngine;
using UnityEngine.UI;

namespace TowerOfBlocks.MVP.View
{
    /// <summary>
    /// Wrapper over horizontal ScrollRect for the bottom blocks strip.
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public sealed class BlocksScrollView : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;

        public ScrollRect ScrollRect => scrollRect;

        private void Awake()
        {
            if (scrollRect == null)
            {
                scrollRect = GetComponent<ScrollRect>();
            }
        }
    }
}

