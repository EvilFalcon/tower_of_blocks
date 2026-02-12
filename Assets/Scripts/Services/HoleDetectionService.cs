using UnityEngine;

namespace Services
{
    /// <summary>
    /// Implementation that checks if a point is inside hole RectTransform bounds.
    /// </summary>
    public sealed class HoleDetectionService : IHoleDetectionService
    {
        private readonly RectTransform _holeRect;

        public HoleDetectionService(RectTransform holeRect)
        {
            _holeRect = holeRect;
        }

        public bool IsPointInside(Vector2 screenPosition)
        {
            if (_holeRect == null)
            {
                return false;
            }

            return RectTransformUtility.RectangleContainsScreenPoint(_holeRect, screenPosition);
        }
    }
}
