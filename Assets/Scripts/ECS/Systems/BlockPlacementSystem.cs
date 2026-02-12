using ECS.Components;
using Leopotam.EcsProto;
using Services;
using UnityEngine;

namespace ECS.Systems
{
    /// <summary>
    /// Decides what happens when a dragged block is released.
    /// </summary>
    public sealed class BlockPlacementSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;
        private readonly IHoleDetectionService _holeDetectionService;
        private readonly ITowerBoundsService _towerBoundsService;

        public BlockPlacementSystem(
            IHoleDetectionService holeDetectionService,
            ITowerBoundsService towerBoundsService)
        {
            _holeDetectionService = holeDetectionService;
            _towerBoundsService = towerBoundsService;
        }

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (ProtoEntity entity in _aspect.DragIt)
            {
                ref var drag = ref _aspect.DragPool.Get(entity);

                if (!drag.IsDragging && _aspect.BlockPool.Has(entity))
                {
                    if (_holeDetectionService.IsPointInside(drag.PointerScreenPosition))
                    {
                        ref var block = ref _aspect.BlockPool.Get(entity);
                        block.IsInScroll = false;
                        _aspect.DragPool.Del(entity);
                        _aspect.AnimationPool.Add(entity);
                    }
                    else if (_towerBoundsService.IsPointInside(drag.PointerWorldPosition))
                    {
                        if (!_aspect.TowerBlockPool.Has(entity))
                        {
                            ref var towerBlock = ref _aspect.TowerBlockPool.Add(entity);
                            towerBlock.TowerIndex = GetTowerHeight();

                            ref var block = ref _aspect.BlockPool.Get(entity);
                            block.IsInScroll = false;

                            if (_aspect.PositionPool.Has(entity))
                            {
                                ref var pos = ref _aspect.PositionPool.Get(entity);
                                pos.Value = new Vector2(
                                    Random.Range(_towerBoundsService.LeftX, _towerBoundsService.RightX),
                                    _towerBoundsService.BottomY + towerBlock.TowerIndex * 1f);
                            }
                        }

                        _aspect.DragPool.Del(entity);
                    }
                    else
                    {
                        ref var block = ref _aspect.BlockPool.Get(entity);
                        block.IsInScroll = true;
                        _aspect.DragPool.Del(entity);
                    }
                }
            }
        }

        private int GetTowerHeight()
        {
            int height = 0;
            foreach (ProtoEntity entity in _aspect.TowerIt)
            {
                if (_aspect.TowerBlockPool.Has(entity))
                {
                    ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);
                    if (towerBlock.TowerIndex >= height)
                    {
                        height = towerBlock.TowerIndex + 1;
                    }
                }
            }
            return height;
        }
    }
}