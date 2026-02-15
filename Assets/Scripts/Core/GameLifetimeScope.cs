using ECS;
using Leopotam.EcsProto;
using MVP.Presenter;
using MVP.View;
using Services;
using Configuration;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private GameplayView _gameplayView;
        [SerializeField] private BlockView _blockPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ProtoWorld>(resolver =>
            {
                var world = new ProtoWorld(new GameAspect());
                return world;
            }, Lifetime.Singleton);

            builder.Register<GameAspect>(resolver =>
            {
                var world = resolver.Resolve<ProtoWorld>();
                return (GameAspect)world.Aspect(typeof(GameAspect));
            }, Lifetime.Singleton);

            builder.RegisterEntryPoint<EcsRootWorld>(Lifetime.Singleton);

            builder.Register<IGameConfigProvider>(resolver => new ScriptableObjectConfigProvider(_gameConfig), Lifetime.Singleton);

            builder.Register<ITowerBoundsService>(resolver => new TowerBoundsService(
                _gameplayView.TowerArea,
                resolver.Resolve<IGameConfigProvider>(),
                _gameplayView.MainCanvas,
                _gameplayView.MainCamera), Lifetime.Singleton);

            builder.Register<IHoleDetectionService>(resolver => new HoleDetectionService(
                _gameplayView.HoleArea,
                _gameplayView.MainCanvas,
                _gameplayView.MainCamera), Lifetime.Singleton);

            builder.Register<Transform>(resolver =>
            {
                GameObject poolParent = new GameObject("BlockViewPool");
                poolParent.transform.SetParent(_gameplayView.transform);
                return poolParent.transform;
            }, Lifetime.Singleton);

            builder.Register<IBlockViewPool>(resolver => new BlockViewPool(
                resolver.Resolve<BlockView>(),
                resolver.Resolve<Transform>()), Lifetime.Singleton);

            builder.Register<PooledBlockPresenterManager>(resolver => new PooledBlockPresenterManager(
                resolver.Resolve<GameAspect>(),
                resolver.Resolve<IGameConfigProvider>()), Lifetime.Singleton);

            builder.Register<IBlockPoolService>(resolver => new BlockPoolService(
                resolver.Resolve<GameAspect>(),
                resolver.Resolve<IBlockViewPool>(),
                resolver.Resolve<PooledBlockPresenterManager>(),
                resolver.Resolve<IGameConfigProvider>(),
                _gameplayView.MainCanvas,
                _gameplayView.MainCamera), Lifetime.Singleton);

            builder.Register<IBlockDragService>(resolver => new BlockDragService(
                resolver.Resolve<GameAspect>(),
                () => resolver.Resolve<TowerBlockPresenterManager>(),
                resolver.Resolve<GameplayView>().DragGhostView,
                resolver.Resolve<IGameConfigProvider>(),
                _gameplayView.MainCamera), Lifetime.Singleton);

            builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
            builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

            builder.Register<IGameStateLoader>(resolver => new GameStateLoader(
                resolver.Resolve<GameAspect>(),
                resolver.Resolve<ISaveService>(),
                resolver.Resolve<ITowerBoundsService>(),
                resolver.Resolve<IGameConfigProvider>()), Lifetime.Singleton);

            builder.RegisterInstance(_gameplayView);
            builder.RegisterInstance(_blockPrefab);

            builder.Register<BlocksScrollPresenter>(resolver => new BlocksScrollPresenter(
                _gameplayView.BlocksScrollView,
                resolver.Resolve<GameAspect>(),
                resolver.Resolve<IGameConfigProvider>(),
                resolver.Resolve<IBlockDragService>(),
                resolver.Resolve<BlockView>(),
                _gameplayView.MainCanvas,
                _gameplayView.MainCamera), Lifetime.Singleton);

            builder.Register<MessagePresenter>(resolver => new MessagePresenter(
                _gameplayView.MessageView,
                resolver.Resolve<ILocalizationService>()), Lifetime.Singleton);

            builder.Register<DragGhostPresenter>(resolver => new DragGhostPresenter(
                _gameplayView.DragGhostView,
                resolver.Resolve<IGameConfigProvider>()), Lifetime.Singleton);

            builder.Register<TowerBlockPresenterManager>(resolver => new TowerBlockPresenterManager(
                resolver.Resolve<GameAspect>(),
                _gameplayView.TowerArea,
                resolver.Resolve<IBlockViewPool>(),
                resolver.Resolve<IGameConfigProvider>(),
                resolver.Resolve<IBlockDragService>(),
                _gameplayView.MainCanvas,
                _gameplayView.MainCamera), Lifetime.Singleton);

            builder.RegisterEntryPoint<GameplayPresenter>(Lifetime.Singleton);
        }
    }
}
