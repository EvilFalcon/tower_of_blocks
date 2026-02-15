using System;
using Configuration;
using ECS.Systems;
using Leopotam.EcsProto;
using MVP.Presenter;
using Services;
using VContainer.Unity;

namespace Core
{
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
        private readonly ISaveService _saveService;
        private readonly IBlockPoolService _poolService;
        private readonly IBlockViewPool _viewPool;
        private readonly PooledBlockPresenterManager _pooledPresenterManager;
        private readonly TowerBlockPresenterManager _towerPresenterManager;
        private readonly IGameConfigProvider _configProvider;

        private IProtoSystems _updateSystems;
        private IProtoSystems _fixedSystems;
        private IProtoSystems _lateSystems;

        public EcsRootWorld(
            ProtoWorld world,
            IHoleDetectionService holeDetectionService,
            ITowerBoundsService towerBoundsService,
            ISaveService saveService,
            IBlockPoolService poolService,
            IBlockViewPool viewPool,
            PooledBlockPresenterManager pooledPresenterManager,
            TowerBlockPresenterManager towerPresenterManager,
            IGameConfigProvider configProvider)
        {
            _world = world;
            _holeDetectionService = holeDetectionService;
            _towerBoundsService = towerBoundsService;
            _saveService = saveService;
            _poolService = poolService;
            _viewPool = viewPool;
            _pooledPresenterManager = pooledPresenterManager;
            _towerPresenterManager = towerPresenterManager;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            _updateSystems = new ProtoSystems(_world)
                .AddSystem(new DragInitSystem())
                .AddSystem(new BlockDragSystem(_pooledPresenterManager))
                .AddSystem(new BlockPlacementSystem(_holeDetectionService, _towerBoundsService, _configProvider))
                .AddSystem(new TowerManagementSystem(_towerBoundsService))
                .AddSystem(new BlockRemovalSystem(_towerBoundsService, _configProvider))
                .AddSystem(new AnimationSystem(_poolService, _viewPool, _pooledPresenterManager, _towerPresenterManager));

            _fixedSystems = new ProtoSystems(_world);

            _lateSystems = new ProtoSystems(_world)
                .AddSystem(new SaveSystem(_saveService, _towerBoundsService, _configProvider));

            _updateSystems.Init();
            _fixedSystems.Init();
            _lateSystems.Init();
        }

        public void Tick() =>
            _updateSystems?.Run();

        public void FixedTick() =>
            _fixedSystems?.Run();

        public void LateTick() =>
            _lateSystems?.Run();

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

