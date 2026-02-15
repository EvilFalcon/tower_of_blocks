using System;
using System.Collections.Generic;

namespace ECS.Models
{
    [Serializable]
    public sealed class TowerSaveData
    {
        public const int CurrentFormatVersion = 3;

        public int FormatVersion = CurrentFormatVersion;

        [Serializable]
        public sealed class TowerBlockData
        {
            public int BlockId;
            public int TowerIndex;
            public float LocalX;
            public float RelativeX;
            public float PositionX;
            public float PositionY;
        }

        public List<TowerBlockData> TowerBlocks = new();
    }
}
