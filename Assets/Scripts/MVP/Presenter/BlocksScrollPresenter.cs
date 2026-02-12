using Configuration;
using ECS;
using Leopotam.EcsProto;
using MVP.View;
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

        public BlocksScrollPresenter(BlocksScrollView view, GameAspect aspect, IGameConfigProvider configProvider)
        {
            _view = view;
            _aspect = aspect;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            for (int i = 0; i < _configProvider.BlocksCount; i++)
            {
                ref var block = ref _aspect.BlockPool.NewEntity(out ProtoEntity entity);
                block.Id = _configProvider.BlockColorIds[i];
                block.IsInScroll = true;

                ref var pos = ref _aspect.PositionPool.Add(entity);
                pos.Value = Vector2.zero;
            }
        }
    }
}
