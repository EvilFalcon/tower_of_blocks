using ECS.Components;
using Leopotam.EcsProto;
using Services;
using UnityEngine;

namespace ECS.Systems
{
    /// <summary>
    /// Maintains tower structure.
    /// </summary>
    public sealed class TowerManagementSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;
        private readonly ITowerBoundsService _towerBoundsService;

        public TowerManagementSystem(ITowerBoundsService towerBoundsService)
        {
            _towerBoundsService = towerBoundsService;
        }

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (ProtoEntity entity in _aspect.TowerIt)
            {
                if (_aspect.TowerBlockPool.Has(entity) && _aspect.PositionPool.Has(entity))
                {
                    ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);
                    ref var pos = ref _aspect.PositionPool.Get(entity);

                    float targetY = _towerBoundsService.BottomY + towerBlock.TowerIndex * 1f;

                    if (_aspect.AnimationPool.Has(entity))
                    {
                        ref var anim = ref _aspect.AnimationPool.Get(entity);
                        if (anim.Type == AnimationType.CollapseDown)
                        {
                            if (Mathf.Abs(pos.Value.y - targetY) < 0.01f)
                            {
                                _aspect.AnimationPool.Del(entity);
                            }
                        }
                    }

                    if (Mathf.Abs(pos.Value.y - targetY) > 0.01f)
                    {
                        pos.Value = new Vector2(pos.Value.x, targetY);
                    }
                }
            }
        }
    }
}