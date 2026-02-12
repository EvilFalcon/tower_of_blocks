using Leopotam.EcsProto;

namespace TowerOfBlocks.ECS.Systems
{
    /// <summary>
    /// Maintains tower structure.
    /// </summary>
    public sealed class TowerManagementSystem : IProtoInitSystem, IProtoRunSystem
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

