using System;
using Configuration;
using Core.Signals;
using MVP.Interfaces;
using R3;
using Services;

namespace MVP.Presenter
{
    public sealed class DragGhostPresenter : IDisposable
    {
        private readonly IDragGhostView _view;
        private readonly IGameConfigProvider _configProvider;
        private readonly CompositeDisposable _disposables = new();

        public DragGhostPresenter(IDragGhostView view, IGameConfigProvider configProvider)
        {
            _view = view;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            GameSignals.BlockPlaced
                .Subscribe(_ =>
                {
                    var duration = _configProvider.Animations.MissDisappear.FadeDuration;
                    _view?.HideWithAnimation(duration);
                })
                .AddTo(_disposables);
            
            GameSignals.BlockRemoved
                .Subscribe(_ =>
                {
                    var duration = _configProvider.Animations.MissDisappear.FadeDuration;
                    _view?.HideWithAnimation(duration);
                })
                .AddTo(_disposables);

            GameSignals.BlockMissed
                .Subscribe(_ =>
                {
                    var duration = _configProvider.Animations.MissDisappear.FadeDuration;
                    _view?.HideWithAnimation(duration);
                })
                .AddTo(_disposables);

            GameSignals.TowerHeightLimitReached
                .Subscribe(_ =>
                {
                    var duration = _configProvider.Animations.MissDisappear.FadeDuration;
                    _view?.HideWithAnimation(duration);
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
