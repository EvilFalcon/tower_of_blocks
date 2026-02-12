using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    /// <summary>
    /// ScriptableObject-based implementation of IGameConfigProvider.
    /// </summary>
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
    }
}
