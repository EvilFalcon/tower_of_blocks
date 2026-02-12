using System;
using ECS.Systems;
using Leopotam.EcsProto;
using Services;
using VContainer.Unity;

namespace Core
{
    /// <summary>
    /// ECS entry point driven by VContainer.
    /// </summary>
    public sealed class EcsRootWorld :
        IInitializable,
        ITickable,
        IFixedTickable,
        ILateTickable,
        IDisposable
    {
        private readonly ProtoWorld _world;
        private readonly IHoleDetectionService _holeDetectionService;
        private readonly ITowerBoundsService _towerBoundsService;

        private IProtoSystems _updateSystems;
        private IProtoSystems _fixedSystems;
        private IProtoSystems _lateSystems;

        public EcsRootWorld(
            ProtoWorld world,
            IHoleDetectionService holeDetectionService,
            ITowerBoundsService towerBoundsService)
        {
            _world = world;
            _holeDetectionService = holeDetectionService;
            _towerBoundsService = towerBoundsService;
        }

        /// <summary>
        /// Called by VContainer once after the container is built.
        /// Here we create and initialize all ECS pipelines.
        /// </summary>
        public void Initialize()
        {
            _updateSystems = new ProtoSystems(_world)
                .AddSystem(new BlockDragSystem())
                .AddSystem(new BlockPlacementSystem(_holeDetectionService, _towerBoundsService))
                .AddSystem(new TowerManagementSystem(_towerBoundsService))
                .AddSystem(new BlockRemovalSystem())
                .AddSystem(new AnimationSystem());

            _fixedSystems = new ProtoSystems(_world);

            _lateSystems = new ProtoSystems(_world)
                .AddSystem(new SaveSystem());

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

