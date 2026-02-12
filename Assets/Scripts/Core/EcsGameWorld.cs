using Core;
using ECS;
using VContainer;
using VContainer.Unity;
using Leopotam.EcsProto;

namespace TowerOfBlocks.Core
{
    /// <summary>
    /// VContainer composition root for the game.
    /// </summary>
    public sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register(resolver =>
            {
                var world = new ProtoWorld(new GameAspect());
                return world;
            }, Lifetime.Singleton);

            builder.RegisterEntryPoint<EcsRootWorld>(Lifetime.Singleton);
        }
    }
}

