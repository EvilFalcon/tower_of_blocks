using TowerOfBlocks.MVP.Interfaces;

namespace TowerOfBlocks.MVP.Presenter
{
    /// <summary>
    /// Presenter for a block in the tower.
    /// </summary>
    public sealed class TowerBlockPresenter
    {
        private readonly ITowerBlockView _view;

        public TowerBlockPresenter(ITowerBlockView view)
        {
            _view = view;
        }
    }
}

