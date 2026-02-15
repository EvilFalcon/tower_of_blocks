using System;
using Configuration;
using Core.Signals;
using ECS.Models;
using Leopotam.EcsProto;
using R3;
using Services;
using UnityEngine;
using Utils;

namespace ECS.Systems
{
    public sealed class SaveSystem : IProtoInitSystem, IProtoDestroySystem
    {
        private GameAspect _aspect;
        private readonly ISaveService _saveService;
        private readonly ITowerBoundsService _towerBoundsService;
        private readonly IGameConfigProvider _configProvider;
        private IDisposable _subscription;

        public SaveSystem(ISaveService saveService, ITowerBoundsService towerBoundsService, IGameConfigProvider configProvider)
        {
            _saveService = saveService;
            _towerBoundsService = towerBoundsService;
            _configProvider = configProvider;
        }

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));

            _subscription = GameSignals.EcsStateChanged
                .Subscribe(_ => SaveState());

            GameSignals.SaveRequested.Subscribe(_ => SaveState());
        }

        private void SaveState()
        {
            if (_aspect == null || _towerBoundsService == null)
            {
                return;
            }

            var saveData = new TowerSaveData();
            var savedBlocksCount = 0;

            try
            {
                foreach (var entity in _aspect.TowerIt)
                {
                    if (!_aspect.TowerBlockPool.Has(entity) ||
                        !_aspect.BlockPool.Has(entity) ||
                        !_aspect.PositionPool.Has(entity))
                        continue;

                    ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);
                    ref var block = ref _aspect.BlockPool.Get(entity);
                    ref var pos = ref _aspect.PositionPool.Get(entity);

                    var localX = _towerBoundsService.AnchoredXToLocalX(pos.Value.x);
                    var leftX = _towerBoundsService.LeftX;
                    var rightX = _towerBoundsService.RightX;
                    var span = Mathf.Max(FloatConstants.MinSpanEpsilon, rightX - leftX);
                    var relativeX = Mathf.Clamp01((localX - leftX) / span);

                    var blockData = new TowerSaveData.TowerBlockData
                    {
                        BlockId = block.Id,
                        TowerIndex = towerBlock.TowerIndex,
                        LocalX = localX,
                        RelativeX = relativeX
                    };

                    saveData.TowerBlocks.Add(blockData);
                    savedBlocksCount++;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            var json = JsonUtility.ToJson(saveData);
            _saveService.Save(_configProvider.SaveKey, json);
        }

        public void Destroy()
        {
            _subscription?.Dispose();
            _subscription = null;
            _aspect = null;
        }
    }
}