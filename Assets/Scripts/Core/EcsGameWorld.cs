using UnityEngine;
using VContainer;
using VContainer.Unity;
using Leopotam.EcsProto;
using TowerOfBlocks.ECS;

namespace TowerOfBlocks.Core
{
    /// <summary>
    /// VContainer composition root for the game.
    /// </summary>
    public sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ProtoWorld>(resolver =>
            {
                var world = new ProtoWorld(new GameAspect());
                return world;
            }, Lifetime.Singleton);

            builder.RegisterEntryPoint<EcsRootWorld>(Lifetime.Singleton);
        }
    }
}

