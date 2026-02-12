using Leopotam.EcsProto;
using TowerOfBlocks.ECS;
using VContainer;
using VContainer.Unity;

namespace Core
{
    /// <summary>
    /// VContainer composition root for the game.
    /// Attach this component to a root GameObject in the main scene.
    /// </summary>
    public sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register ECS world as a singleton.
            builder.Register<ProtoWorld>(resolver =>
            {
                var world = new ProtoWorld(new GameAspect());
                return world;
            }, Lifetime.Singleton);

            // Register ECS root entry point that will drive all systems.
            builder.RegisterEntryPoint<EcsRootWorld>(Lifetime.Singleton);
        }
    }
}

