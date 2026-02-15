using Configuration;
using ECS;
using ECS.Components;
using Leopotam.EcsProto;
using MVP.Presenter;
using MVP.View;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Services
{
    public sealed class BlockPoolService : IBlockPoolService
    {
        private readonly GameAspect _aspect;
        private readonly IBlockViewPool _viewPool;
        private readonly PooledBlockPresenterManager _presenterManager;
        private readonly IGameConfigProvider _configProvider;
        private readonly Canvas _canvas;
        private readonly Camera _camera;

        public BlockPoolService(GameAspect aspect, IBlockViewPool viewPool, PooledBlockPresenterManager presenterManager, IGameConfigProvider configProvider,
            Canvas canvas, Camera camera)
        {
            _aspect = aspect;
            _viewPool = viewPool;
            _presenterManager = presenterManager;
            _configProvider = configProvider;
            _canvas = canvas;
            _camera = camera;
        }

        public (ProtoEntity pooledEntity, BlockView pooledView) CreatePooledBlock(
            ProtoEntity originalScrollEntity,
            Vector2 originalPosition,
            Sprite sprite,
            Transform parent,
            IBlockDragService dragService,
            ScrollRect scrollRect = null,
            bool createView = true)
        {
            ref var pooledBlock = ref _aspect.BlockPool.NewEntity(out ProtoEntity pooledEntity);

            if (_aspect.BlockPool.Has(originalScrollEntity))
            {
                ref var originalBlock = ref _aspect.BlockPool.Get(originalScrollEntity);
                pooledBlock.Id = originalBlock.Id;
                pooledBlock.IsInScroll = false;
            }

            ref var pooled = ref _aspect.PooledBlockPool.Add(pooledEntity);
            pooled.OriginalEntity = originalScrollEntity;
            pooled.OriginalPosition = originalPosition;
            pooled.WasInScroll = _aspect.BlockPool.Has(originalScrollEntity) &&
                                 _aspect.BlockPool.Get(originalScrollEntity).IsInScroll;

            ref var pos = ref _aspect.PositionPool.Add(pooledEntity);
            pos.Value = originalPosition;

            if (!createView)
                return (pooledEntity, null);

            var pooledView = _viewPool.Get();
            pooledView.transform.SetParent(parent);
            pooledView.transform.SetAsLastSibling();

            pooledView.SetImage(sprite);

            pooledView.SetPosition(originalPosition);

            if (!pooled.WasInScroll)
                pooledView.Image?.SetAlpha(_configProvider.DragGhostAlpha);

            var entityRef = pooledView.EntityRef;
            
            entityRef.Entity = pooledEntity;

            var dragHandler = pooledView.DragHandler;
            
            dragHandler.Initialize(dragService, _canvas, _camera);

            if (scrollRect != null)
                dragHandler.SetScrollRect(scrollRect);

            _presenterManager.CreatePresenter(pooledEntity, pooledView);

            return (pooledEntity, pooledView);
        }

        public void ReturnPooledBlock(ProtoEntity pooledEntity, BlockView pooledView)
        {
            if (pooledView != null && pooledView.DragHandler != null)
                pooledView.DragHandler.ReenableScrollRect();
            _presenterManager.RemovePresenter(pooledEntity);

            if (_aspect.DragPool.Has(pooledEntity))
                _aspect.DragPool.Del(pooledEntity);

            if (_aspect.AnimationPool.Has(pooledEntity))
                _aspect.AnimationPool.Del(pooledEntity);

            if (_aspect.PositionPool.Has(pooledEntity))
                _aspect.PositionPool.Del(pooledEntity);

            if (_aspect.PooledBlockPool.Has(pooledEntity))
                _aspect.PooledBlockPool.Del(pooledEntity);

            if (_aspect.BlockPool.Has(pooledEntity))
                _aspect.BlockPool.Del(pooledEntity);

            if (pooledView == null)
                return;

            pooledView.ResetState();
            _viewPool.Return(pooledView);
        }
    }
}