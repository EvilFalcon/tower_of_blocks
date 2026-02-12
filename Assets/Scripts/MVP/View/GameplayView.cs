using MVP.Interfaces;
using UnityEngine;

namespace MVP.View
{
    /// <summary>
    /// Unified view component for all gameplay UI elements.
    /// </summary>
    public sealed class GameplayView : MonoBehaviour
    {
        [SerializeField] private BlocksScrollView _blocksScrollView;
        [SerializeField] private RectTransform _holeArea;
        [SerializeField] private MessageView _messageView;

        public BlocksScrollView BlocksScrollView => _blocksScrollView;
        public RectTransform HoleArea => _holeArea;
        public IMessageView MessageView => _messageView;
    }
}
