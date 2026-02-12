using ECS;
using Leopotam.EcsProto;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// Implementation that converts UI drag events into ECS DragComponent changes.
    /// </summary>
    public sealed class BlockDragService : IBlockDragService
    {
        private readonly GameAspect _aspect;
        private readonly Camera _camera;

        public BlockDragService(GameAspect aspect, Camera camera)
        {
            _aspect = aspect;
            _camera = camera ?? Camera.main;
        }

        public void OnDragStarted(ProtoEntity entity, Vector2 screenPosition)
        {
            if (!_aspect.BlockPool.Has(entity))
            {
                return;
            }

            if (!_aspect.DragPool.Has(entity))
            {
                ref var drag = ref _aspect.DragPool.Add(entity);
                drag.IsDragging = true;
                drag.StartPosition = ScreenToWorld(screenPosition);
                drag.PointerWorldPosition = ScreenToWorld(screenPosition);
                drag.PointerScreenPosition = screenPosition;
            }
        }

        public void OnDrag(Vector2 screenPosition)
        {
            foreach (ProtoEntity entity in _aspect.DragIt)
            {
                if (_aspect.DragPool.Has(entity))
                {
                    ref var drag = ref _aspect.DragPool.Get(entity);
                    drag.PointerWorldPosition = ScreenToWorld(screenPosition);
                    drag.PointerScreenPosition = screenPosition;
                }
            }
        }

        public void OnDragEnded(Vector2 screenPosition)
        {
            foreach (ProtoEntity entity in _aspect.DragIt)
            {
                if (_aspect.DragPool.Has(entity))
                {
                    ref var drag = ref _aspect.DragPool.Get(entity);
                    drag.IsDragging = false;
                    drag.PointerWorldPosition = ScreenToWorld(screenPosition);
                    drag.PointerScreenPosition = screenPosition;
                }
            }
        }

        private Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            if (_camera != null)
            {
                return _camera.ScreenToWorldPoint(screenPosition);
            }

            return screenPosition;
        }
    }
}
