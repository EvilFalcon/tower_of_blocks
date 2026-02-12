using ECS;
using Leopotam.EcsProto;
using MVP.Interfaces;
using R3;

namespace MVP.Presenter
{
    /// <summary>
    /// Presenter for a single block view.
    /// </summary>
    public sealed class BlockPresenter
    {
        private readonly IBlockView _view;
        private readonly ProtoEntity _entity;
        private readonly GameAspect _aspect;
        private CompositeDisposable _disposables = new();

        public BlockPresenter(IBlockView view, ProtoEntity entity, GameAspect aspect)
        {
            _view = view;
            _entity = entity;
            _aspect = aspect;
        }

        public void Initialize()
        {
            if (_aspect.PositionPool.Has(_entity))
            {
                ref var pos = ref _aspect.PositionPool.Get(_entity);
                _view.SetPosition(pos.Value);
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
