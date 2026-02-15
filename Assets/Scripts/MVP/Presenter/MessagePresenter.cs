using Core.Signals;
using MVP.Interfaces;
using R3;
using Services;
using System;
using Cysharp.Threading.Tasks;

namespace MVP.Presenter
{
    public sealed class MessagePresenter : IDisposable
    {
        private readonly IMessageView _view;
        private readonly ILocalizationService _localizationService;
        private CompositeDisposable _disposables = new();
        private const float MessageDisplayDuration = 2f;

        public MessagePresenter(
            IMessageView view,
            ILocalizationService localizationService)
        {
            _view = view;
            _localizationService = localizationService;
        }

        public void Initialize()
        {
            GameSignals.BlockPlaced
                .Subscribe(_ => ShowMessage("BlockPlaced"))
                .AddTo(_disposables);

            GameSignals.BlockRemoved
                .Subscribe(_ => ShowMessage("BlockRemoved"))
                .AddTo(_disposables);

            GameSignals.BlockMissed
                .Subscribe(_ => ShowMessage("BlockMissed"))
                .AddTo(_disposables);

            GameSignals.TowerHeightLimitReached
                .Subscribe(_ => ShowMessage("TowerHeightLimitReached"))
                .AddTo(_disposables);
        }

        private void ShowMessage(string key)
        {
            var text = _localizationService.GetText(key);
            _view.ShowMessage(text);
            HideMessageAfterDelay().Forget();
        }

        private async UniTaskVoid HideMessageAfterDelay()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(MessageDisplayDuration));
            _view.HideMessage();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
