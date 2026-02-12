using Leopotam.EcsProto;

namespace TowerOfBlocks.ECS.Systems
{
    /// <summary>
    /// Bridges AnimationComponent data to the presentation layer.
    /// </summary>
    public sealed class AnimationSystem : IProtoInitSystem, IProtoRunSystem
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

