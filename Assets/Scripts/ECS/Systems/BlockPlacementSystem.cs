using Configuration;
using Core.Signals;
using ECS.Components;
using Leopotam.EcsProto;
using R3;
using Services;
using UnityEngine;

namespace ECS.Systems
{
    public sealed class BlockPlacementSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;
        private readonly IHoleDetectionService _holeDetectionService;
        private readonly ITowerBoundsService _towerBoundsService;
        private readonly IGameConfigProvider _configProvider;

        public BlockPlacementSystem(
            IHoleDetectionService holeDetectionService,
            ITowerBoundsService towerBoundsService,
            IGameConfigProvider configProvider)
        {
            _holeDetectionService = holeDetectionService;
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
            int blocksAddedThisRun = 0;

            foreach (var entity in _aspect.DragIt)
            {
                ref var drag = ref _aspect.DragPool.Get(entity);
                
                if (drag.IsDragging || !_aspect.DragStatePool.Has(entity))
                    continue;

                ref var state = ref _aspect.DragStatePool.Get(entity);
                
                if (!state.PendingDrop)
                    continue;

                var originalEntity = state.OriginalEntity;
                var wasInScroll = state.WasInScroll;

                var holeInside = _holeDetectionService.IsPointInside(drag.PointerScreenPosition);
                var towerMargin = _towerBoundsService.HalfBlockHeight;
                var towerInside = _towerBoundsService.IsPointInsideWithMargin(drag.PointerScreenPosition, towerMargin);

                if (holeInside)
                {
                    if (!wasInScroll && _aspect.TowerBlockPool.Has(originalEntity))
                    {
                        ref var animOriginal = ref GetOrAddAnimation(originalEntity);
                        animOriginal.Type = AnimationType.HoleFall;
                        animOriginal.RemoveAtTime = Time.time + _configProvider.Animations.Timing.RemoveAfterAnimationSeconds;
                        if (_towerBoundsService.ScreenPointToLocalPoint(drag.PointerScreenPosition, out Vector2 localDrop))
                            animOriginal.TargetPosition = _towerBoundsService.LocalToAnchoredPosition(localDrop.x, localDrop.y);
                    }
                    else
                    {
                        GameSignals.BlockMissed.OnNext(Unit.Default);
                    }

                    state.PendingDrop = false;
                    GameSignals.EcsStateChanged.OnNext(Unit.Default);
                }
                else if (towerInside)
                {
                    if (!wasInScroll)
                    {
                        state.PendingDrop = false;
                        GameSignals.BlockMissed.OnNext(Unit.Default);
                        GameSignals.EcsStateChanged.OnNext(Unit.Default);
                        continue;
                    }

                    var canPlace = _aspect.BlockPool.Has(originalEntity);

                    if (canPlace)
                    {
                        ref var blockCheck = ref _aspect.BlockPool.Get(originalEntity);
                        canPlace = blockCheck.IsInScroll;
                    }

                    if (!canPlace)
                    {
                        state.PendingDrop = false;
                        GameSignals.BlockMissed.OnNext(Unit.Default);
                        GameSignals.EcsStateChanged.OnNext(Unit.Default);
                        continue;
                    }

                    var towerHeight = GetTowerHeight() + blocksAddedThisRun;
                    var halfHeight = _towerBoundsService.HalfBlockHeight;
                    var newBlockY = _towerBoundsService.BottomY + halfHeight + towerHeight * _towerBoundsService.BlockHeight;

                    if (towerHeight > 0 && !IsDropOverTowerTop(drag.PointerScreenPosition, towerHeight))
                    {
                        state.PendingDrop = false;
                        GameSignals.BlockMissed.OnNext(Unit.Default);
                        GameSignals.EcsStateChanged.OnNext(Unit.Default);
                        continue;
                    }

                    if (newBlockY >= _towerBoundsService.TopY)
                    {
                        state.PendingDrop = false;
                        GameSignals.TowerHeightLimitReached.OnNext(Unit.Default);
                        GameSignals.EcsStateChanged.OnNext(Unit.Default);
                        continue;
                    }

                    ref var originalBlock = ref _aspect.BlockPool.Get(originalEntity);

                    ref var newTowerBlock = ref _aspect.BlockPool.NewEntity(out ProtoEntity towerEntity);
                    newTowerBlock.Id = originalBlock.Id;
                    newTowerBlock.IsInScroll = false;

                    if (_aspect.ColorPool.Has(originalEntity))
                    {
                        ref var originalColor = ref _aspect.ColorPool.Get(originalEntity);
                        ref var newColor = ref _aspect.ColorPool.Add(towerEntity);
                        newColor.Value = originalColor.Value;
                    }

                    ref var towerBlock = ref _aspect.TowerBlockPool.Add(towerEntity);
                    towerBlock.TowerIndex = towerHeight;

                    ref var pos = ref _aspect.PositionPool.Add(towerEntity);
                    var localX = GetPlacementLocalX(towerHeight, drag.PointerScreenPosition);
                    pos.Value = _towerBoundsService.LocalToAnchoredPosition(localX, newBlockY);

                    ref var anim = ref _aspect.AnimationPool.Add(towerEntity);
                    anim.Type = AnimationType.PlaceBounce;

                    state.PendingDrop = false;
                    GameSignals.BlockPlaced.OnNext(Unit.Default);
                    blocksAddedThisRun++;
                    GameSignals.EcsStateChanged.OnNext(Unit.Default);
                }
                else
                {
                    state.PendingDrop = false;
                    GameSignals.BlockMissed.OnNext(Unit.Default);
                    GameSignals.EcsStateChanged.OnNext(Unit.Default);
                }
            }
        }

        private ref AnimationComponent GetOrAddAnimation(ProtoEntity entity)
        {
            if (_aspect.AnimationPool.Has(entity))
                return ref _aspect.AnimationPool.Get(entity);

            return ref _aspect.AnimationPool.Add(entity);
        }

        private int GetTowerHeight()
        {
            var height = 0;

            foreach (var entity in _aspect.TowerIt)
            {
                if (!_aspect.TowerBlockPool.Has(entity))
                    continue;

                ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);

                if (towerBlock.TowerIndex >= height)
                    height = towerBlock.TowerIndex + 1;
            }

            return height;
        }

        private float GetPlacementLocalX(int newBlockTowerIndex, Vector2 dropScreenPosition)
        {
            if (newBlockTowerIndex == 0)
            {
                return _towerBoundsService.ScreenPointToLocalX(dropScreenPosition, out float localX) 
                    ? Mathf.Clamp(localX, _towerBoundsService.LeftX, _towerBoundsService.RightX) 
                    : Random.Range(_towerBoundsService.LeftX, _towerBoundsService.RightX);
            }

            var prevLocalX = 0f;

            foreach (var entity in _aspect.TowerIt)
            {
                if (!_aspect.TowerBlockPool.Has(entity) || !_aspect.PositionPool.Has(entity))
                    continue;

                ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);

                if (towerBlock.TowerIndex != newBlockTowerIndex - 1)
                    continue;

                ref var pos = ref _aspect.PositionPool.Get(entity);
                prevLocalX = _towerBoundsService.AnchoredXToLocalX(pos.Value.x);
                break;
            }

            var halfEdge = _towerBoundsService.HalfBlockHeight;
            var offset = Random.Range(-halfEdge, halfEdge);
            var newLocalX = Mathf.Clamp(prevLocalX + offset, _towerBoundsService.LeftX, _towerBoundsService.RightX);
           
            return newLocalX;
        }

        private bool IsDropOverTowerTop(Vector2 dropScreenPosition, int currentTowerHeight)
        {
            if (currentTowerHeight <= 0)
                return true;
           
            if (!_towerBoundsService.ScreenPointToLocalPoint(dropScreenPosition, out Vector2 localPoint))
                return false;

            var halfHeight = _towerBoundsService.HalfBlockHeight;
            var topBlockCenterY = _towerBoundsService.BottomY + halfHeight + (currentTowerHeight - 1) * _towerBoundsService.BlockHeight;
            var topBlockBottomY = topBlockCenterY - halfHeight;
            
            return localPoint.y >= topBlockBottomY - halfHeight;
        }
    }
}