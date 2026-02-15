using UnityEngine;

namespace Configuration
{
    public interface IGameConfigProvider
    {
        int BlocksCount { get; }
        Sprite GetSprite(int index);

        TowerBoundsSettings TowerBounds { get; }
        AnimationSettings Animations { get; }
        string SaveKey { get; }
        float DragGhostAlpha { get; }
    }
}
