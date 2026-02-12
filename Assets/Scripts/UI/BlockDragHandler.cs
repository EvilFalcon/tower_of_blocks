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
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_parentScrollRect != null)
            {
                _parentScrollRect.enabled = true;
            }
        }
    }
}
