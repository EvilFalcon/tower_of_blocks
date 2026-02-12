using MVP.View;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// UI event handler for block drag operations.
    /// </summary>
    public sealed class BlockDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ScrollRect _parentScrollRect;
        private IBlockDragService _dragService;
        private EntityReference _entityRef;

        private void Awake()
        {
            _entityRef = GetComponent<EntityReference>();
        }

        public void Initialize(IBlockDragService dragService)
        {
            _dragService = dragService;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_parentScrollRect == null)
            {
                _parentScrollRect = GetComponentInParent<ScrollRect>();
            }

            if (_parentScrollRect != null)
            {
                _parentScrollRect.enabled = false;
            }

            if (_entityRef != null && _dragService != null)
            {
                _dragService.OnDragStarted(_entityRef.Entity, eventData.position);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragService?.OnDrag(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_parentScrollRect != null)
            {
                _parentScrollRect.enabled = true;
            }

            _dragService?.OnDragEnded(eventData.position);
        }
    }
}
