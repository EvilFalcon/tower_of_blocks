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
        public System.Collections.Generic.IReadOnlyList<int> BlockColorIds => _config.BlockColorIds;
    }
}
