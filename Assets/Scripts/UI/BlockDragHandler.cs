using System;
using Leopotam.EcsProto;
using MVP.Interfaces;
using MVP.View;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public sealed class BlockDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ScrollRect _parentScrollRect;
        private IBlockDragService _dragService;
        private EntityReference _entityRef;
        private Transform _originalParent;
        private Canvas _canvas;
        private BlockView _pooledView;
        private ProtoEntity _pooledEntity;
        private bool _pooledDragActive;
        private BlockView _blockView;
        private Camera _camera;

        private void Awake()
        {
            _blockView = GetComponent<BlockView>();
            _entityRef = _blockView != null ? _blockView.EntityRef : GetComponent<EntityReference>();
        }

        public void Initialize(IBlockDragService dragService, Canvas canvas = null, Camera camera = null)
        {
            _dragService = dragService;
            _canvas = canvas;
            _camera = camera;
        }

        public void SetScrollRect(ScrollRect scrollRect)
        {
            _parentScrollRect = scrollRect;
        }

        public void ReenableScrollRect()
        {
            if (_parentScrollRect != null)
                _parentScrollRect.enabled = true;
        }

        private void ResolveEntityRef()
        {
            if (_entityRef == null)
                _entityRef = _blockView != null ? _blockView.EntityRef : GetComponent<EntityReference>();
        }

        private Camera GetCamera()
        {
            if (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                return null;

            return _canvas != null ? _canvas.worldCamera ?? _camera : _camera;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransform rect = transform as RectTransform;

            if (_pooledView == null)
            {
                if (_parentScrollRect == null)
                    _parentScrollRect = GetComponentInParent<ScrollRect>();

                if (_parentScrollRect != null)
                    _parentScrollRect.enabled = false;
            }

            var clickOffsetInCanvas = Vector2.zero;
            var blockPosAfterMove = Vector2.zero;

            if (rect != null && rect.parent != null)
            {
                _originalParent = rect.parent;

                blockPosAfterMove = rect.anchoredPosition;

                var parentRect = rect.parent as RectTransform;

                if (parentRect != null)
                {
                    var cam = GetCamera();
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parentRect, eventData.position, cam, out Vector2 clickPosLocal);
                    clickOffsetInCanvas = blockPosAfterMove - clickPosLocal;
                }
            }

            ResolveEntityRef();

            if (_entityRef != null && _dragService != null && rect != null)
            {
                var parentRect = rect.parent as RectTransform;

                Sprite sprite = null;
                var image = _blockView != null ? _blockView.Image : GetComponent<Image>();
                if (image != null)
                    sprite = image.sprite;

                var canvasParent = _canvas != null ? _canvas.transform : rect.parent;

                if (canvasParent != null && rect.parent != canvasParent)
                {
                    var canvasRect = canvasParent as RectTransform;

                    if (canvasRect != null)
                    {
                        Vector2 posInCanvas = canvasRect.InverseTransformPoint(rect.position);
                        blockPosAfterMove = posInCanvas;
                        var cam = GetCamera();

                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, cam, out var clickInCanvas))
                            clickOffsetInCanvas = blockPosAfterMove - clickInCanvas;
                    }
                }

                var pooledResult = _dragService.OnDragStarted(
                    _entityRef.Entity,
                    eventData.position,
                    parentRect,
                    blockPosAfterMove,
                    clickOffsetInCanvas,
                    sprite,
                    canvasParent);

                if (pooledResult.HasValue)
                {
                    _pooledEntity = pooledResult.Value.pooledEntity;
                    _pooledView = pooledResult.Value.pooledView;
                    _pooledDragActive = true;
                    if (_pooledView != null)
                        eventData.pointerDrag = _pooledView.gameObject;
                }
                else if (_canvas != null && rect.parent != _canvas.transform)
                {
                    var cam = GetCamera();
                    var canvasRect = _canvas.transform as RectTransform;
                    rect.SetParent(_canvas.transform, true);
                    blockPosAfterMove = rect.anchoredPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvasRect, eventData.position, cam, out Vector2 clickPosLocal);
                    clickOffsetInCanvas = blockPosAfterMove - clickPosLocal;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragService == null)
                return;

            if (_pooledDragActive)
            {
                _dragService.OnDrag(eventData.position, null, null, _pooledEntity, null);
                return;
            }

            var rect = transform as RectTransform;
            if (rect == null)
                return;

            var parentRect1 = rect.parent as RectTransform;
            if (_canvas != null && rect.parent == _canvas.transform)
                parentRect1 = _canvas.transform as RectTransform;

            ResolveEntityRef();
            var view = _blockView as IBlockView;
            if (view != null && _entityRef != null)
                _dragService.OnDrag(eventData.position, rect, parentRect1, _entityRef.Entity, pos => view.SetPosition(pos));
            else
                _dragService.OnDrag(eventData.position, rect, parentRect1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_parentScrollRect != null)
                _parentScrollRect.enabled = true;

            if (_parentScrollRect == null)
                _parentScrollRect = GetComponentInParent<ScrollRect>();

            if (_parentScrollRect != null)
                _parentScrollRect.enabled = true;

            var rect = transform as RectTransform;

            if (_pooledDragActive)
            {
                if (_dragService != null)
                {
                    var parentRect = _pooledView != null && _pooledView.transform != null && _pooledView.transform.parent != null
                        ? _pooledView.transform.parent as RectTransform
                        : (rect != null ? rect.parent as RectTransform : null);
                    if (_canvas != null && parentRect == null)
                        parentRect = _canvas.transform as RectTransform;
                    _dragService.OnDragEnded(eventData.position, parentRect);
                }

                _pooledView = null;
                _pooledEntity = default;
                _pooledDragActive = false;
                return;
            }

            if (_dragService != null && rect != null)
            {
                var parentRect = rect.parent as RectTransform;

                if (_canvas != null && rect.parent == _canvas.transform)
                {
                    parentRect = _canvas.transform as RectTransform;
                }

                _dragService.OnDragEnded(eventData.position, parentRect);
            }

            if (_pooledView == null && rect != null && _originalParent != null && rect.parent != _originalParent)
            {
                rect.SetParent(_originalParent, true);
            }

            _pooledView = null;
            _pooledEntity = default;
            _pooledDragActive = false;
        }
    }
}
