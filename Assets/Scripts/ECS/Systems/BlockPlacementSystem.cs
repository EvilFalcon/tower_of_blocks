using Leopotam.EcsProto;

namespace ECS.Systems
{
    /// <summary>
    /// Decides what happens when a dragged block is released.
    /// </summary>
    public sealed class BlockPlacementSystem : IProtoInitSystem, IProtoRunSystem
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

                if (!drag.IsDragging && _aspect.BlockPool.Has(entity))
                {
                }
            }
        }
    }
}