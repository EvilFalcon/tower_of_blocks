using R3;

namespace TowerOfBlocks.Core.Signals
{
    /// <summary>
    /// Central place for core reactive signals (R3).
    /// This keeps SaveSystem and other services reactive instead of polling each frame.
    /// </summary>
    public static class GameSignals
    {
        /// <summary>
        /// Emitted when the game state should be persisted to storage explicitly
        /// (for example on app pause / quit).
        /// </summary>
        public static readonly Subject<Unit> SaveRequested = new();

        /// <summary>
        /// Emitted when значимое состояние ECS изменилось
        /// (например, изменена структура башни или состояние блоков),
        /// и прогресс стоит сохранить при удобном случае.
        /// Вызывать из ECS-систем, где происходит изменение.
        /// </summary>
        public static readonly Subject<Unit> EcsStateChanged = new();
    }
}

