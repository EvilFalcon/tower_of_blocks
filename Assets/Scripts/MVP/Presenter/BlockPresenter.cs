using TowerOfBlocks.MVP.Interfaces;

namespace TowerOfBlocks.MVP.Presenter
{
    /// <summary>
    /// Presenter for a single block view.
    /// </summary>
    public sealed class BlockPresenter
    {
        private readonly IBlockView _view;

        public BlockPresenter(IBlockView view)
        {
            _view = view;
        }
    }
}

