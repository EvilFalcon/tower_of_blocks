using Configuration;
using ECS;
using ECS.Models;
using Leopotam.EcsProto;
using UnityEngine;

namespace Services
{
    public sealed class GameStateLoader : IGameStateLoader
    {
        private readonly GameAspect _aspect;
        private readonly ISaveService _saveService;
        private readonly ITowerBoundsService _towerBoundsService;
        private readonly IGameConfigProvider _configProvider;

        public GameStateLoader(
            GameAspect aspect,
            ISaveService saveService,
            ITowerBoundsService towerBoundsService,
            IGameConfigProvider configProvider)
        {
            _aspect = aspect;
            _saveService = saveService;
            _towerBoundsService = towerBoundsService;
            _configProvider = configProvider;
        }

        public bool HasSavedState()
        {
            return _saveService.HasKey(_configProvider.SaveKey);
        }

        public void LoadState()
        {
            if (!HasSavedState())
            {
                return;
            }

            var json = _saveService.Load(_configProvider.SaveKey);
            
            if (string.IsNullOrEmpty(json))
                return;

            var saveData = JsonUtility.FromJson<TowerSaveData>(json);

            if (saveData == null || saveData.TowerBlocks == null)
                return;

            var loadedCount = 0;
            
            foreach (var blockData in saveData.TowerBlocks)
            {
                ref var block = ref _aspect.BlockPool.NewEntity(out ProtoEntity entity);
                loadedCount++;
                block.Id = blockData.BlockId;
                block.IsInScroll = false;

                ref var towerBlock = ref _aspect.TowerBlockPool.Add(entity);
                towerBlock.TowerIndex = blockData.TowerIndex;

                Vector2 anchoredPos;
                var isLegacyFormat = saveData.FormatVersion < 2;
                
                if (isLegacyFormat)
                    anchoredPos = new Vector2(blockData.PositionX, blockData.PositionY);
                else
                {
                    var halfHeight = _towerBoundsService.HalfBlockHeight;
                    var newBlockY = _towerBoundsService.BottomY + halfHeight + blockData.TowerIndex * _towerBoundsService.BlockHeight;
                    var leftX = _towerBoundsService.LeftX;
                    var rightX = _towerBoundsService.RightX;
                    float localX;
                    
                    if (saveData.FormatVersion >= 3 && blockData.RelativeX >= 0 && blockData.RelativeX <= 1)
                        localX = Mathf.Lerp(leftX, rightX, blockData.RelativeX);
                    else
                        localX = Mathf.Clamp(blockData.LocalX, leftX, rightX);
                   
                    anchoredPos = _towerBoundsService.LocalToAnchoredPosition(localX, newBlockY);
                }

                ref var pos = ref _aspect.PositionPool.Add(entity);
                pos.Value = anchoredPos;

                if (_configProvider == null)
                    continue;

                ref var color = ref _aspect.ColorPool.Add(entity);
                color.Value = Color.white;
            }
        }
    }
}
