using ECS;
using MVP.View;
using Configuration;
using VContainer.Unity;

namespace MVP.Presenter
{
    /// <summary>
    /// Main presenter that coordinates all gameplay views and ECS systems.
    /// </summary>
    public sealed class GameplayPresenter : IInitializable
    {
        private readonly GameplayView _view;
        private readonly GameAspect _aspect;
        private readonly IGameConfigProvider _configProvider;
        private readonly BlocksScrollPresenter _scrollPresenter;
        private readonly MessagePresenter _messagePresenter;
        private readonly TowerBlockPresenterManager _towerPresenterManager;

        public GameplayPresenter(
            GameplayView view,
            GameAspect aspect,
            IGameConfigProvider configProvider,
            BlocksScrollPresenter scrollPresenter,
            MessagePresenter messagePresenter,
            TowerBlockPresenterManager towerPresenterManager)
        {
            _view = view;
            _aspect = aspect;
            _configProvider = configProvider;
            _scrollPresenter = scrollPresenter;
            _messagePresenter = messagePresenter;
            _towerPresenterManager = towerPresenterManager;
        }

        public void Initialize()
        {
            _scrollPresenter.Initialize();
            _towerPresenterManager.Initialize();
        }
    }
}
