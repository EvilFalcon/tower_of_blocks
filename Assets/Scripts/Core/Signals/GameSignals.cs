using R3;

namespace Core.Signals
{
    public static class GameSignals
    {
        public static readonly Subject<Unit> SaveRequested = new();
        public static readonly Subject<Unit> EcsStateChanged = new();
        public static readonly Subject<Unit> BlockPlaced = new();
        public static readonly Subject<Unit> BlockRemoved = new();
        public static readonly Subject<Unit> BlockMissed = new();
        public static readonly Subject<Unit> TowerHeightLimitReached = new();
    }
}

