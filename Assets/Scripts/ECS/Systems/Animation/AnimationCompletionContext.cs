using MVP.Presenter;
using Services;

namespace ECS.Systems.Animation
{
    public sealed class AnimationCompletionContext
    {
        public AnimationCompletionContext(
            IBlockPoolService poolService,
            IBlockViewPool viewPool,
            PooledBlockPresenterManager pooledPresenterManager,
            TowerBlockPresenterManager towerPresenterManager)
        {
            PoolService = poolService;
            ViewPool = viewPool;
            PooledPresenterManager = pooledPresenterManager;
            TowerPresenterManager = towerPresenterManager;
        }

        public GameAspect Aspect { get; set; }
        public IBlockPoolService PoolService { get; }
        public IBlockViewPool ViewPool { get; }
        public PooledBlockPresenterManager PooledPresenterManager { get; }
        public TowerBlockPresenterManager TowerPresenterManager { get; }
    }
}
