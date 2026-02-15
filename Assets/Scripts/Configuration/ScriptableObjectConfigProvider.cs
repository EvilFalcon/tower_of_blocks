using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    public sealed class ScriptableObjectConfigProvider : IGameConfigProvider
    {
        private readonly GameConfig _config;

        public ScriptableObjectConfigProvider(GameConfig config)
        {
            _config = config;
        }

        public int BlocksCount => _config.BlocksCount;
        public IReadOnlyList<Sprite> BlockSprites => _config.BlockSprites;
        public Sprite GetSprite(int index) => _config.GetSprite(index);
        public TowerBoundsSettings TowerBounds => _config.TowerBounds;
        public AnimationSettings Animations => _config.Animations;
        public string SaveKey => _config.SaveKey;
        public float DragGhostAlpha => _config.DragGhostAlpha;
    }
}
