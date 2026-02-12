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
    /// <summary>
    /// VContainer composition root for the game.
    /// </summary>
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private TowerBoundsConfig _towerBoundsConfig;
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

            builder.RegisterInstance(_towerBoundsConfig);

            builder.Register<ITowerBoundsService>(resolver => new TowerBoundsService(
                _gameplayView.TowerArea,
                _towerBoundsConfig,
                Camera.main), Lifetime.Singleton);

            builder.Register<IHoleDetectionService>(resolver => new HoleDetectionService(_gameplayView.HoleArea), Lifetime.Singleton);

            builder.Register<IBlockDragService>(resolver => new BlockDragService(
                resolver.Resolve<GameAspect>(),
                Camera.main), Lifetime.Singleton);

            builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
            builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);

            builder.RegisterInstance(_gameplayView);
            builder.RegisterInstance(_blockPrefab);

            builder.Register<BlocksScrollPresenter>(resolver => new BlocksScrollPresenter(
                _gameplayView.BlocksScrollView,
                resolver.Resolve<GameAspect>(),
                resolver.Resolve<IGameConfigProvider>(),
                resolver.Resolve<IBlockDragService>(),
                resolver.Resolve<BlockView>()), Lifetime.Singleton);

            builder.Register<MessagePresenter>(resolver => new MessagePresenter(_gameplayView.MessageView), Lifetime.Singleton);

            builder.Register<TowerBlockPresenterManager>(resolver => new TowerBlockPresenterManager(
                resolver.Resolve<GameAspect>(),
                _gameplayView.TowerArea,
                resolver.Resolve<BlockView>(),
                resolver.Resolve<IGameConfigProvider>()), Lifetime.Singleton);

            builder.RegisterEntryPoint<GameplayPresenter>(Lifetime.Singleton);
        }
    }
}
