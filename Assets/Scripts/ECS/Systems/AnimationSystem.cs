using ECS.Components;
using Leopotam.EcsProto;

namespace ECS.Systems
{
    /// <summary>
    /// Bridges AnimationComponent data to the presentation layer.
    /// AnimationComponent marks entities that need animation.
    /// Actual animations are executed by Presenter → View (LitMotion).
    /// This system just manages the component lifecycle.
    /// </summary>
    public sealed class AnimationSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (ProtoEntity entity in _aspect.BlockIt)
            {
                if (_aspect.AnimationPool.Has(entity))
                {
                    ref var anim = ref _aspect.AnimationPool.Get(entity);

                    if (anim.Type == AnimationType.MissDisappear)
                    {
                        _aspect.AnimationPool.Del(entity);
                        _aspect.BlockPool.Del(entity);
                    }
                }
            }
        }
    }
}
