using Core.Signals;
using ECS.Components;
using Leopotam.EcsProto;
using R3;

namespace ECS.Systems
{
    /// <summary>
    /// Handles removal of blocks.
    /// </summary>
    public sealed class BlockRemovalSystem : IProtoInitSystem, IProtoRunSystem
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

                    if (anim.Type == AnimationType.HoleFall)
                    {
                        if (_aspect.TowerBlockPool.Has(entity))
                        {
                            ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);
                            int removedIndex = towerBlock.TowerIndex;

                            UpdateTowerIndicesAbove(removedIndex);

                            _aspect.TowerBlockPool.Del(entity);
                        }

                        _aspect.AnimationPool.Del(entity);
                        GameSignals.EcsStateChanged.OnNext(Unit.Default);
                    }
                }
            }
        }

        private void UpdateTowerIndicesAbove(int removedIndex)
        {
            foreach (ProtoEntity entity in _aspect.TowerIt)
            {
                if (_aspect.TowerBlockPool.Has(entity))
                {
                    ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);

                    if (towerBlock.TowerIndex > removedIndex)
                    {
                        towerBlock.TowerIndex--;

                        if (_aspect.AnimationPool.Has(entity))
                        {
                            ref var anim = ref _aspect.AnimationPool.Get(entity);
                            anim.Type = AnimationType.CollapseDown;
                        }
                        else
                        {
                            ref var anim = ref _aspect.AnimationPool.Add(entity);
                            anim.Type = AnimationType.CollapseDown;
                        }
                    }
                }
            }
        }
    }
}
