using UnityEngine;

namespace Services
{
    public sealed class HoleDetectionService : IHoleDetectionService
    {
        private readonly RectTransform _holeRect;
        private readonly Camera _camera;

        public HoleDetectionService(RectTransform holeRect, Canvas canvas, Camera camera)
        {
            _holeRect = holeRect;
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                _camera = null;
            else
                _camera = camera ?? (canvas != null ? canvas.worldCamera : null);
        }

        public bool IsPointInside(Vector2 screenPosition)
        {
            if (_holeRect == null)
                return false;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _holeRect, screenPosition, _camera, out var localPoint))
                return false;

            var r = _holeRect.rect;
            var semiX = r.width * 0.5f;
            var semiY = r.height * 0.5f;
            
            if (semiX <= 0f || semiY <= 0f)
                return false;

            var cx = r.center.x;
            var cy = r.center.y;
            var dx = (localPoint.x - cx) / semiX;
            var dy = (localPoint.y - cy) / semiY;

            return dx * dx + dy * dy <= 1f;
        }
    }
}
