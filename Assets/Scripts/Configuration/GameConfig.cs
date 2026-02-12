using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    /// <summary>
    /// Main game configuration: blocks count and their color IDs.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "TowerOfBlocks/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [SerializeField] private int blocksCount = 20;
        [SerializeField] private List<int> blockColorIds = new();

        public int BlocksCount => blocksCount;
        public IReadOnlyList<int> BlockColorIds => blockColorIds;
    }
}
