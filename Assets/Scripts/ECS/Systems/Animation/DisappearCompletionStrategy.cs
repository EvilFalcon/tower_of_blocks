using Leopotam.EcsProto;
using MVP.View;
using Utils;

namespace ECS.Systems.Animation
{
    public sealed class DisappearCompletionStrategy : IAnimationCompletionStrategy
    {
        public void Complete(ProtoEntity entity, AnimationCompletionContext context)
        {
            var aspect = context.Aspect;

            if (aspect.PooledBlockPool.Has(entity))
                CompletePooledBlock(entity, context);
            else
                CompleteTowerBlock(entity, context);
        }

        private static void CompletePooledBlock(ProtoEntity entity, AnimationCompletionContext context)
        {
            var aspect = context.Aspect;
            var pooledView = context.PooledPresenterManager.GetView(entity) as BlockView;

            if (pooledView != null)
            {
                RestoreOriginalViewAlphaIfNeeded(entity, context);
                context.PoolService.ReturnPooledBlock(entity, pooledView);
            }
            else
            {
                RestoreOriginalViewAlphaIfNeeded(entity, context);
                context.PooledPresenterManager.RemovePresenter(entity);
                
                if (aspect.DragPool.Has(entity))
                    aspect.DragPool.Del(entity);
                
                aspect.AnimationPool.Del(entity);
                
                if (aspect.PositionPool.Has(entity))
                    aspect.PositionPool.Del(entity);
                
                aspect.PooledBlockPool.Del(entity);
                aspect.BlockPool.Del(entity);
            }
        }

        private static void RestoreOriginalViewAlphaIfNeeded(ProtoEntity entity, AnimationCompletionContext context)
        {
            var aspect = context.Aspect;
            
            if (!aspect.PooledBlockPool.Has(entity))
                return;

            ref var pooled = ref aspect.PooledBlockPool.Get(entity);
            
            if (pooled.WasInScroll || !context.TowerPresenterManager.HasPresenter(pooled.OriginalEntity))
                return;

            var origView = context.TowerPresenterManager.GetView(pooled.OriginalEntity);
            origView?.Image?.RestoreFullAlphaAndRaycast();
        }

        private static void CompleteTowerBlock(ProtoEntity entity, AnimationCompletionContext context)
        {
            var aspect = context.Aspect;

            if (context.TowerPresenterManager.HasPresenter(entity))
            {
                var towerView = context.TowerPresenterManager.GetView(entity);

                if (towerView is TowerBlockView tv && tv.BlockView != null)
                {
                    tv.BlockView.ResetState();
                    context.ViewPool.Return(tv.BlockView);
                }

                context.TowerPresenterManager.RemovePresenter(entity);
            }

            aspect.AnimationPool.Del(entity);
            aspect.BlockPool.Del(entity);
        }
    }
}
