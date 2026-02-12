using TowerOfBlocks.MVP.View;

namespace TowerOfBlocks.MVP.Presenter
{
    /// <summary>
    /// Presenter for the bottom blocks scroll view.
    /// </summary>
    public sealed class BlocksScrollPresenter
    {
        private readonly BlocksScrollView _view;

        public BlocksScrollPresenter(BlocksScrollView view)
        {
            _view = view;
        }
    }
}

