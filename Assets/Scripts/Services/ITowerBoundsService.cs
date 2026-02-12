using UnityEngine;

namespace Services
{
    /// <summary>
    /// Service for calculating tower area bounds from UI.
    /// </summary>
    public interface ITowerBoundsService
    {
        float TopY { get; }
        float BottomY { get; }
        float LeftX { get; }
        float RightX { get; }
        bool IsPointInside(Vector2 worldPosition);
    }
}
