using UnityEngine;

namespace Services
{
    /// <summary>
    /// Service for mapping block ID to sprite.
    /// </summary>
    public interface IBlockSpriteService
    {
        Sprite GetSprite(int blockId);
    }
}
