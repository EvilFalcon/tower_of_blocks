using Leopotam.EcsProto;
using MVP.Presenter;

namespace ECS.Systems
{
    public sealed class BlockDragSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;
        private readonly PooledBlockPresenterManager _presenterManager;

        public BlockDragSystem(PooledBlockPresenterManager presenterManager)
        {
            _presenterManager = presenterManager;
        }

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (var entity in _aspect.DragIt)
            {
                ref var drag = ref _aspect.DragPool.Get(entity);

                if (!drag.IsDragging)
                    continue;

                if (_aspect.DragStatePool.Has(entity))
                {
                    if (_aspect.PositionPool.Has(entity))
                    {
                        ref var pos = ref _aspect.PositionPool.Get(entity);
                        pos.Value = drag.PointerWorldPosition;
                    }

                    continue;
                }

                if (_aspect.PositionPool.Has(entity))
                {
                    ref var pos = ref _aspect.PositionPool.Get(entity);
                    pos.Value = drag.PointerWorldPosition;
                }

                var view = _presenterManager?.GetView(entity);

                view?.SetPosition(drag.PointerWorldPosition);
            }
        }
    }
}