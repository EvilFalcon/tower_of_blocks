using UnityEngine;

namespace Services
{
    /// <summary>
    /// Service for calculating tower area bounds from UI.
    /// </summary>
    public interface ITowerBoundsService
    {
        /// <summary>
        /// TODO: Will be used for tower height limit check in TowerManagementSystem.
        /// </summary>
        float TopY { get; }

        float BottomY { get; }
        float LeftX { get; }
        float RightX { get; }
        bool IsPointInside(Vector2 worldPosition);
    }
}