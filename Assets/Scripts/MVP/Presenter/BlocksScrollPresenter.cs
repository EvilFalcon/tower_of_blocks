using ECS;
using Leopotam.EcsProto;
using MVP.View;
using Services;
using Configuration;
using UI;
using UnityEngine;

namespace MVP.Presenter
{
    /// <summary>
    /// Presenter for the bottom blocks scroll view.
    /// </summary>
    public sealed class BlocksScrollPresenter
    {
        private readonly BlocksScrollView _view;
        private readonly GameAspect _aspect;
        private readonly IGameConfigProvider _configProvider;
        private readonly IBlockDragService _dragService;
        private readonly BlockView _blockPrefab;

        public BlocksScrollPresenter(
            BlocksScrollView view,
            GameAspect aspect,
            IGameConfigProvider configProvider,
            IBlockDragService dragService,
            BlockView blockPrefab)
        {
            _view = view;
            _aspect = aspect;
            _configProvider = configProvider;
            _dragService = dragService;
            _blockPrefab = blockPrefab;
        }

        public void Initialize()
        {
            Transform content = _view.ScrollRect.content;

            for (int i = 0; i < _configProvider.BlocksCount; i++)
            {
                ref var block = ref _aspect.BlockPool.NewEntity(out ProtoEntity entity);
                block.Id = i;
                block.IsInScroll = true;

                ref var pos = ref _aspect.PositionPool.Add(entity);
                pos.Value = Vector2.zero;

                CreateBlockView(entity, content, i);
            }
        }

        private void CreateBlockView(ProtoEntity entity, Transform parent, int blockIndex)
        {
            BlockView blockView = Object.Instantiate(_blockPrefab, parent);
            GameObject blockObj = blockView.gameObject;

            EntityReference entityRef = blockObj.GetComponent<EntityReference>();
            entityRef.Entity = entity;

            BlockDragHandler dragHandler = blockObj.GetComponent<BlockDragHandler>();
            dragHandler.Initialize(_dragService);

            Sprite sprite = _configProvider.GetSprite(blockIndex);

            if (sprite != null)
            {
                blockView.SetImage(sprite);
            }
        }
    }
}