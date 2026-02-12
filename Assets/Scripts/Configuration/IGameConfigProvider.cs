using System.Collections.Generic;

namespace Configuration
{
    /// <summary>
    /// Provider interface for game configuration.
    /// Allows different sources (ScriptableObject, JSON, remote, etc.).
    /// </summary>
    public interface IGameConfigProvider
    {
        int BlocksCount { get; }
        IReadOnlyList<int> BlockColorIds { get; }
    }
}
