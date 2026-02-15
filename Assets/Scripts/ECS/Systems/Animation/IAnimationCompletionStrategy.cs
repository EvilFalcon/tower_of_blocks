using Leopotam.EcsProto;

namespace ECS.Systems.Animation
{
    public interface IAnimationCompletionStrategy
    {
        void Complete(ProtoEntity entity, AnimationCompletionContext context);
    }
}
