using Leopotam.EcsProto;

namespace TowerOfBlocks.ECS.Systems
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
        }
    }
}

