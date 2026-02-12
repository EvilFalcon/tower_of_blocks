namespace TowerOfBlocks.Core
{
    /// <summary>
    /// High-level game state.
    /// Can be extended later (e.g. Menu, GameOver).
    /// </summary>
    public enum GameState
    {
        None = 0,
        Loading = 1,
        Playing = 2,
        Paused = 3
    }
}

