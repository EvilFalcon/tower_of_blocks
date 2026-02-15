using System;
using Configuration;
using ECS;
using Leopotam.EcsProto;
using MVP.Interfaces;
using MVP.Presenter;
using MVP.View;
using UnityEngine;
using Utils;

namespace Services
{
    public sealed class BlockDragService : IBlockDragService
    {
        private readonly GameAspect _aspect;
        private readonly Func<TowerBlockPresenterManager> _getTowerPresenterManager;
        private readonly IDragGhostView _dragGhostView;
        private readonly IGameConfigProvider _configProvider;
        private readonly Camera _fallbackCamera;

        public BlockDragService(GameAspect aspect, Func<TowerBlockPresenterManager> getTowerPresenterManager, IDragGhostView dragGhostView, IGameConfigProvider configProvider, Camera fallbackCamera)
        {
            _aspect = aspect;
            _getTowerPresenterManager = getTowerPresenterManager;
            _dragGhostView = dragGhostView;
            _configProvider = configProvider;
            _fallbackCamera = fallbackCamera;
        }

        public (ProtoEntity pooledEntity, BlockView pooledView)? OnDragStarted(ProtoEntity entity, Vector2 screenPosition, RectTransform parentRect,
            Vector2 blockCurrentPosition, Vector2 clickOffset, Sprite sprite, Transform parent)
        {
            var dragEntity = GetDragEntity();
            if (!_aspect.DragStatePool.Has(dragEntity))
                return null;

            ref var d = ref _aspect.DragPool.Get(dragEntity);

            if (d.IsDragging)
            {
                d.IsDragging = false;

                if (_aspect.DragStatePool.Has(dragEntity))
                {
                    ref var state = ref _aspect.DragStatePool.Get(dragEntity);

                    if (!state.WasInScroll && _getTowerPresenterManager() != null && _getTowerPresenterManager().HasPresenter(state.OriginalEntity))
                    {
                        var origView = _getTowerPresenterManager().GetView(state.OriginalEntity);

                        var img = origView.Image;

                        if (img != null)
                            img.RestoreFullAlphaAndRaycast();
                    }
                }

                _dragGhostView?.Hide();
            }

            if (!_aspect.BlockPool.Has(entity))
                return null;

            ref var block = ref _aspect.BlockPool.Get(entity);
            var wasInScroll = block.IsInScroll;

            ref var dragState = ref _aspect.DragStatePool.Get(dragEntity);
            dragState.OriginalEntity = entity;
            dragState.WasInScroll = wasInScroll;
            dragState.PendingDrop = true;

            var ghostParent = _dragGhostView?.RectTransform?.parent as RectTransform;
            var cursorInGhostParent = ScreenPointToLocalInRect(ghostParent, screenPosition);
            var startBlockPos = ghostParent != null ? cursorInGhostParent : blockCurrentPosition;

            if (_aspect.PositionPool.Has(dragEntity))
            {
                ref var pos = ref _aspect.PositionPool.Get(dragEntity);
                pos.Value = startBlockPos;
            }

            if (_dragGhostView != null)
                _dragGhostView.Show(sprite, startBlockPos, _configProvider.DragGhostAlpha);

            ref var drag = ref _aspect.DragPool.Get(dragEntity);
            drag.IsDragging = true;
            drag.PointerWorldPosition = startBlockPos;
            drag.PointerScreenPosition = screenPosition;

            return (dragEntity, null);
        }

        private ProtoEntity GetDragEntity()
        {
            if (_aspect.DragStatePool.Has(_aspect.DragEntity))
                return _aspect.DragEntity;
            
            foreach (var e in _aspect.DragStateIt)
                return e;
            
            return default;
        }

        public void OnDrag(Vector2 screenPosition, RectTransform blockRect, RectTransform parentRect, ProtoEntity? entityForImmediateApply = null,
            Action<Vector2> applyPositionImmediate = null)
        {
            var dragEntity = GetDragEntity();
            if (!_aspect.DragStatePool.Has(dragEntity) || !_aspect.DragPool.Has(dragEntity))
                return;

            ref var drag = ref _aspect.DragPool.Get(dragEntity);
            if (!drag.IsDragging)
                return;

            var ghostParent = _dragGhostView?.RectTransform?.parent as RectTransform;
            var cursorInGhostParent = ScreenPointToLocalInRect(ghostParent, screenPosition);

            drag.PointerWorldPosition = cursorInGhostParent;
            drag.PointerScreenPosition = screenPosition;

            if (_dragGhostView != null && ghostParent != null)
                _dragGhostView.SetPosition(cursorInGhostParent);
        }

        public void OnDragEnded(Vector2 screenPosition, RectTransform parentRect)
        {
            var dragEntity = GetDragEntity();

            if (!_aspect.DragStatePool.Has(dragEntity) || !_aspect.DragPool.Has(dragEntity))
                return;

            ref var drag = ref _aspect.DragPool.Get(dragEntity);
            drag.IsDragging = false;
            drag.PointerWorldPosition = ScreenToUI(screenPosition, parentRect);
            drag.PointerScreenPosition = screenPosition;
        }

        public bool IsTowerBlock(ProtoEntity entity)
        {
            return _aspect.TowerBlockPool.Has(entity);
        }

        private Camera GetCameraForCanvas(Canvas canvas)
        {
            if (canvas == null) return _fallbackCamera;

            return canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : (canvas.worldCamera ?? _fallbackCamera);
        }

        private Vector2 ScreenToUI(Vector2 screenPosition, RectTransform parentRect)
        {
            return ScreenPointToLocalInRect(parentRect, screenPosition);
        }

        private Vector2 ScreenPointToLocalInRect(RectTransform rect, Vector2 screenPosition)
        {
            if (rect == null)
                return screenPosition;
            var canvas = rect.GetComponentInParent<Canvas>();
            var cam = GetCameraForCanvas(canvas);
           
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect, screenPosition, cam, out Vector2 localPoint);
            return localPoint;
        }
    }
}