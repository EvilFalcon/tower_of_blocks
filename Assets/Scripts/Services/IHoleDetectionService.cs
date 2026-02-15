using UnityEngine;

namespace Services
{
    public interface IHoleDetectionService
    {
        bool IsPointInside(Vector2 screenPosition);
    }
}
