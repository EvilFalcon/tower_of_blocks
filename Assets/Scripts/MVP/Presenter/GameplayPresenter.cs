using Services;
using VContainer.Unity;

namespace MVP.Presenter
{
    public sealed class GameplayPresenter : IInitializable
    {
        private readonly BlocksScrollPresenter _scrollPresenter;
        private readonly MessagePresenter _messagePresenter;
        private readonly DragGhostPresenter _dragGhostPresenter;
        private readonly TowerBlockPresenterManager _towerPresenterManager;
        private readonly PooledBlockPresenterManager _pooledPresenterManager;
        private readonly IGameStateLoader _stateLoader;

        public GameplayPresenter(
            BlocksScrollPresenter scrollPresenter,
            MessagePresenter messagePresenter,
            DragGhostPresenter dragGhostPresenter,
            TowerBlockPresenterManager towerPresenterManager,
            PooledBlockPresenterManager pooledPresenterManager,
            IGameStateLoader stateLoader)
        {
            _scrollPresenter = scrollPresenter;
            _messagePresenter = messagePresenter;
            _dragGhostPresenter = dragGhostPresenter;
            _towerPresenterManager = towerPresenterManager;
            _pooledPresenterManager = pooledPresenterManager;
            _stateLoader = stateLoader;
        }

        public void Initialize()
        {
            if (_stateLoader.HasSavedState())
            {
                _stateLoader.LoadState();
            }

            _scrollPresenter.Initialize();
            _towerPresenterManager.Initialize();
            _pooledPresenterManager.Initialize();
            _messagePresenter.Initialize();
            _dragGhostPresenter.Initialize();
        }
    }
}
