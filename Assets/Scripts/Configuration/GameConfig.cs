using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configuration
{
    /// <summary>
    /// Main game configuration: blocks sprites.
    /// Count is determined by array length, IDs are array indices.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "TowerOfBlocks/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [SerializeField] private Sprite[] _blockSprites = new Sprite[20];

        public int BlocksCount => _blockSprites.Length;
        public IReadOnlyList<Sprite> BlockSprites => _blockSprites;
        public Sprite GetSprite(int index) => index >= 0 && index < _blockSprites.Length ? _blockSprites[index] : null;
    }
}
