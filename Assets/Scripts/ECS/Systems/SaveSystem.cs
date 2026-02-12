using System;
using Leopotam.EcsProto;
using R3;
using TowerOfBlocks.Core.Signals;

namespace TowerOfBlocks.ECS.Systems
{
    /// <summary>
    /// Reactive save system on R3.
    /// </summary>
    public sealed class SaveSystem : IProtoInitSystem, IProtoDestroySystem
    {
        private GameAspect _aspect;
        private IDisposable _subscription;

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));

            _subscription = GameSignals.EcsStateChanged.Subscribe(_ =>
            {
            });
        }

        public void Destroy()
        {
            _subscription?.Dispose();
            _subscription = null;
            _aspect = null;
        }
    }
}

