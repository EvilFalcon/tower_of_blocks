using TowerOfBlocks.MVP.Interfaces;

namespace TowerOfBlocks.MVP.Presenter
{
    /// <summary>
    /// Presenter for game messages.
    /// </summary>
    public sealed class MessagePresenter
    {
        private readonly IMessageView _view;

        public MessagePresenter(IMessageView view)
        {
            _view = view;
        }
    }
}

