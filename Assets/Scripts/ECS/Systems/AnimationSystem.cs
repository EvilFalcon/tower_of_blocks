using System.Collections.Generic;
using ECS.Components;
using ECS.Systems.Animation;
using Leopotam.EcsProto;
using MVP.Presenter;
using Services;
using UnityEngine;

namespace ECS.Systems
{
    public sealed class AnimationSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;
        private readonly AnimationCompletionContext _context;
        private readonly IReadOnlyDictionary<AnimationType, IAnimationCompletionStrategy> _strategies;

        public AnimationSystem(
            IBlockPoolService poolService,
            IBlockViewPool viewPool,
            PooledBlockPresenterManager presenterManager,
            TowerBlockPresenterManager towerPresenterManager)
        {
            _context = new AnimationCompletionContext(
                poolService, viewPool, presenterManager, towerPresenterManager);

            var disappearStrategy = new DisappearCompletionStrategy();
            _strategies = new Dictionary<AnimationType, IAnimationCompletionStrategy>
            {
                { AnimationType.MissDisappear, disappearStrategy },
                { AnimationType.HoleFall, disappearStrategy }
            };
        }

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
            _context.Aspect = _aspect;
        }

        public void Run()
        {
            foreach (var entity in _aspect.BlockIt)
            {
                if (!_aspect.AnimationPool.Has(entity))
                    continue;

                ref var anim = ref _aspect.AnimationPool.Get(entity);

                if (!_strategies.TryGetValue(anim.Type, out var strategy))
                    continue;

                if (!(anim.RemoveAtTime > 0f) || !(Time.time >= anim.RemoveAtTime))
                    continue;

                strategy.Complete(entity, _context);
            }
        }
    }
}
