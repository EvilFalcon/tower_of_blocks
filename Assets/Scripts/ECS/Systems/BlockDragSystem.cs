using Leopotam.EcsProto;

namespace TowerOfBlocks.ECS.Systems
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
        }
    }
}

