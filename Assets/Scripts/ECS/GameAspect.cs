using Leopotam.EcsProto;

namespace TowerOfBlocks.ECS
{
    /// <summary>
    /// Root aspect for the main game world.
    /// Component pools will be added here step-by-step
    /// as gameplay features are implemented.
    /// </summary>
    public sealed class GameAspect : IProtoAspect
    {
        private ProtoWorld _world;

        public void Init(ProtoWorld world)
        {
            _world = world;
            _world.AddAspect(this);
        }

        public void PostInit()
        {
        }

        /// <summary>
        /// Provides access to the ECS world associated with this aspect.
        /// </summary>
        public ProtoWorld World() => _world;
    }
}

