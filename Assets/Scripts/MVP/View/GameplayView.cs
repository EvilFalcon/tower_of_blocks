using MVP.Interfaces;
using UnityEngine;

namespace MVP.View
{
    public sealed class GameplayView : MonoBehaviour
    {
        [SerializeField] private BlocksScrollView _blocksScrollView;
        [SerializeField] private RectTransform _holeArea;
        [SerializeField] private RectTransform _towerArea;
        [SerializeField] private MessageView _messageView;
        [SerializeField] private DragGhostView _dragGhostView;
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Camera _mainCamera;

        public BlocksScrollView BlocksScrollView => _blocksScrollView;
        public RectTransform HoleArea => _holeArea;
        public RectTransform TowerArea => _towerArea;
        public IMessageView MessageView => _messageView;
        public IDragGhostView DragGhostView => _dragGhostView;
        public Canvas MainCanvas => _mainCanvas;
        public Camera MainCamera => _mainCamera;
    }
}