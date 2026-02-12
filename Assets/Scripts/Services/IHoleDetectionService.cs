using UnityEngine;

namespace Services
{
    /// <summary>
    /// Service for detecting if a point is inside the hole area.
    /// </summary>
    public interface IHoleDetectionService
    {
        bool IsPointInside(Vector2 screenPosition);
    }
}
