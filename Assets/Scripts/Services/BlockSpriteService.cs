using UnityEngine;

namespace Services
{
    /// <summary>
    /// Maps block ID to sprite from a pre-configured array.
    /// </summary>
    public sealed class BlockSpriteService : IBlockSpriteService
    {
        private readonly Sprite[] _sprites;

        public BlockSpriteService(Sprite[] sprites)
        {
            _sprites = sprites;
        }

        public Sprite GetSprite(int blockId)
        {
            if (blockId < 0 || blockId >= _sprites.Length)
            {
                return null;
            }

            return _sprites[blockId];
        }
    }
}
