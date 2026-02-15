using Configuration;
using Core.Signals;
using ECS.Components;
using Leopotam.EcsProto;
using R3;
using Services;
using UnityEngine;

namespace ECS.Systems
{
    public sealed class BlockRemovalSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;
        private readonly ITowerBoundsService _towerBoundsService;
        private readonly IGameConfigProvider _configProvider;

        public BlockRemovalSystem(ITowerBoundsService towerBoundsService, IGameConfigProvider configProvider)
        {
            _towerBoundsService = towerBoundsService;
            _configProvider = configProvider;
        }

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (var entity in _aspect.BlockIt)
            {
                if (!_aspect.AnimationPool.Has(entity))
                    continue;

                ref var anim = ref _aspect.AnimationPool.Get(entity);

                if (anim.Type != AnimationType.HoleFall)
                    continue;

                var towerEntity = entity;

                if (_aspect.PooledBlockPool.Has(entity))
                {
                    ref var pooled = ref _aspect.PooledBlockPool.Get(entity);

                    if (!pooled.WasInScroll)
                        towerEntity = pooled.OriginalEntity;
                }

                if (_aspect.TowerBlockPool.Has(towerEntity))
                {
                    ref var towerBlock = ref _aspect.TowerBlockPool.Get(towerEntity);
                    var removedIndex = towerBlock.TowerIndex;

                    UpdateTowerIndicesAbove(removedIndex);

                    _aspect.TowerBlockPool.Del(towerEntity);

                    GameSignals.BlockRemoved.OnNext(Unit.Default);
                    var removeAfter = _configProvider.Animations.Timing.RemoveAfterAnimationSeconds;
                    anim.RemoveAtTime = Time.time + removeAfter;
                    GameSignals.EcsStateChanged.OnNext(Unit.Default);
                }
                else if (anim.RemoveAtTime <= 0f)
                    anim.RemoveAtTime = Time.time + _configProvider.Animations.Timing.RemoveAfterAnimationSeconds;
            }
        }

        private void UpdateTowerIndicesAbove(int removedIndex)
        {
            foreach (var entity in _aspect.TowerIt)
            {
                if (!_aspect.TowerBlockPool.Has(entity))
                    continue;

                ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);

                if (towerBlock.TowerIndex <= removedIndex)
                    continue;

                towerBlock.TowerIndex--;

                var halfHeight = _towerBoundsService.HalfBlockHeight;
                var targetY = _towerBoundsService.BottomY + halfHeight + towerBlock.TowerIndex * _towerBoundsService.BlockHeight;
                var anchoredY = _towerBoundsService.LocalToAnchoredPosition(0f, targetY).y;

                if (_aspect.PositionPool.Has(entity))
                {
                    ref var pos = ref _aspect.PositionPool.Get(entity);
                    Vector2 oldPos = pos.Value;
                    pos.Value = new Vector2(pos.Value.x, anchoredY);
                }

                if (_aspect.DragPool.Has(entity))
                {
                    ref var drag = ref _aspect.DragPool.Get(entity);

                    if (drag.IsDragging)
                        continue;
                }

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