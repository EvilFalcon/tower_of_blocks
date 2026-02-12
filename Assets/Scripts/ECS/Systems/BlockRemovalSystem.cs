using Leopotam.EcsProto;

namespace ECS.Systems
{
    /// <summary>
    /// Handles removal of blocks.
    /// </summary>
    public sealed class BlockRemovalSystem : IProtoInitSystem, IProtoRunSystem
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
