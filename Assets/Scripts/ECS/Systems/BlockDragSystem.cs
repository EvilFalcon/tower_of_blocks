using ECS.Components;
using Leopotam.EcsProto;

namespace ECS.Systems
{
    /// <summary>
    /// Handles drag state for blocks.
    /// </summary>
    public sealed class BlockDragSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (ProtoEntity entity in _aspect.DragIt)
            {
                ref var drag = ref _aspect.DragPool.Get(entity);

                if (drag.IsDragging)
                {
                    if (_aspect.PositionPool.Has(entity))
                    {
                        ref var pos = ref _aspect.PositionPool.Get(entity);
                        pos.Value = drag.PointerWorldPosition;
                    }
                }
            }
        }
    }
}