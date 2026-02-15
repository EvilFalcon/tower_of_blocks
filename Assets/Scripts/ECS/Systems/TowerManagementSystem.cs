using ECS.Components;
using Leopotam.EcsProto;
using Services;
using UnityEngine;
using Utils;

namespace ECS.Systems
{
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
            foreach (var entity in _aspect.TowerIt)
            {
                if (!_aspect.TowerBlockPool.Has(entity) || !_aspect.PositionPool.Has(entity))
                    continue;

                ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);
                ref var pos = ref _aspect.PositionPool.Get(entity);

                var halfHeight = _towerBoundsService.HalfBlockHeight;
                var targetY = _towerBoundsService.BottomY + halfHeight + towerBlock.TowerIndex * _towerBoundsService.BlockHeight;
                var anchoredY = _towerBoundsService.LocalToAnchoredPosition(0f, targetY).y;

                if (_aspect.AnimationPool.Has(entity))
                {
                    ref var anim = ref _aspect.AnimationPool.Get(entity);
                    
                    if (anim.Type == AnimationType.CollapseDown)
                    {
                        if (Mathf.Abs(pos.Value.y - anchoredY) < FloatConstants.PositionEpsilon)
                            _aspect.AnimationPool.Del(entity);
                    }
                }
                
                if (Mathf.Abs(pos.Value.y - anchoredY) > FloatConstants.PositionEpsilon)
                    pos.Value = new Vector2(pos.Value.x, anchoredY);
            }
        }
    }
}