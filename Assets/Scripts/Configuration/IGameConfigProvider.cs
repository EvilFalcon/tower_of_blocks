using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    /// <summary>
    /// Provider interface for game configuration.
    /// Allows different sources (ScriptableObject, JSON, remote, etc.).
    /// </summary>
    public interface IGameConfigProvider
    {
        int BlocksCount { get; }
        IReadOnlyList<Sprite> BlockSprites { get; }
        Sprite GetSprite(int index);
    }
}
