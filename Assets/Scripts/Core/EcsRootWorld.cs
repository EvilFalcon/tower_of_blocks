using System;
using Leopotam.EcsProto;
using VContainer.Unity;

namespace TowerOfBlocks.Core
{
    /// <summary>
    /// ECS entry point driven by VContainer.
    /// Responsible for creating and running system pipelines for Update / FixedUpdate / LateUpdate.
    /// </summary>
    public sealed class EcsRootWorld :
        IInitializable,
        ITickable,
        IFixedTickable,
        ILateTickable,
        IDisposable
    {
        private readonly ProtoWorld _world;

        private IProtoSystems _updateSystems;
        private IProtoSystems _fixedSystems;
        private IProtoSystems _lateSystems;

        public EcsRootWorld(ProtoWorld world)
        {
            _world = world;
        }

        /// <summary>
        /// Called by VContainer once after the container is built.
        /// Here we create and initialize all ECS pipelines.
        /// </summary>
        public void Initialize()
        {
            // Single world, separated system groups for different Unity loops.
            _updateSystems = new ProtoSystems(_world);
            _fixedSystems = new ProtoSystems(_world);
            _lateSystems = new ProtoSystems(_world);

            // TODO: Register gameplay modules, systems and services here
            // in the following steps of the implementation.
            //
            // Example:
            // _updateSystems
            //     .AddSystem(new SomeUpdateSystem());
            //
            // _fixedSystems
            //     .AddSystem(new SomeFixedSystem());
            //
            // _lateSystems
            //     .AddSystem(new SomeCleanupSystem());

            _updateSystems.Init();
            _fixedSystems.Init();
            _lateSystems.Init();
        }

        public void Tick()
        {
            _updateSystems?.Run();
        }

        public void FixedTick()
        {
            _fixedSystems?.Run();
        }

        public void LateTick()
        {
            _lateSystems?.Run();
        }

        public void Dispose()
        {
            _updateSystems?.Destroy();
            _fixedSystems?.Destroy();
            _lateSystems?.Destroy();

            _updateSystems = null;
            _fixedSystems = null;
            _lateSystems = null;

            _world?.Destroy();
        }
    }
}

