using Configuration;
using UnityEngine;

namespace Services
{
    public sealed class TowerBoundsService : ITowerBoundsService
    {
        private readonly RectTransform _towerAreaRect;
        private readonly IGameConfigProvider _configProvider;
        private readonly Camera _camera;

        public TowerBoundsService(RectTransform towerAreaRect, IGameConfigProvider configProvider, Canvas canvas, Camera camera)
        {
            _towerAreaRect = towerAreaRect;
            _configProvider = configProvider;
            
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                _camera = null;
            else
                _camera = camera ?? (canvas != null ? canvas.worldCamera : null);
        }

        private TowerBoundsSettings Bounds => _configProvider.TowerBounds;

        public float TopY
        {
            get
            {
                var rect = _towerAreaRect.rect;
                var height = rect.height;
                var topEdge = Mathf.Max(rect.yMin, rect.yMax);
                
                return topEdge - height * Bounds.TopMarginPercent;
            }
        }

        public float BottomY
        {
            get
            {
                var rect = _towerAreaRect.rect;
                var height = rect.height;
                var bottomEdge = Mathf.Min(rect.yMin, rect.yMax);
                
                return bottomEdge + height * Bounds.BottomMarginPercent;
            }
        }

        public float LeftX
        {
            get
            {
                if (_towerAreaRect == null)
                    return 0f;
                
                var rect = _towerAreaRect.rect;
                var width = rect.width;
                var leftEdge = Mathf.Min(rect.xMin, rect.xMax);
                
                return leftEdge + width * Bounds.HorizontalMarginPercent;
            }
        }

        public float RightX
        {
            get
            {
                if (_towerAreaRect == null)
                    return 0f;
                
                var rect = _towerAreaRect.rect;
                var width = rect.width;
                var rightEdge = Mathf.Max(rect.xMin, rect.xMax);
                
                return rightEdge - width * Bounds.HorizontalMarginPercent;
            }
        }

        public float BlockHeight => Bounds.BlockHeight;
        public float HalfBlockHeight => Bounds.BlockHeight * 0.5f;

        public bool IsPointInside(Vector2 screenPosition) =>
            _towerAreaRect != null && RectTransformUtility.RectangleContainsScreenPoint(_towerAreaRect, screenPosition, _camera);

        public bool IsPointInsideWithMargin(Vector2 screenPosition, float marginInLocalUnits)
        {
            if (_towerAreaRect == null)
                return false;
            
            if (!ScreenPointToLocalPoint(screenPosition, out Vector2 localPoint))
                return false;
            
            var left = LeftX - marginInLocalUnits;
            var right = RightX + marginInLocalUnits;
            var bottom = BottomY - marginInLocalUnits;
            var top = TopY + marginInLocalUnits;
            
            return localPoint.x >= left && localPoint.x <= right && localPoint.y >= bottom && localPoint.y <= top;
        }

        public Vector2 LocalToAnchoredPosition(float localX, float localY)
        {
            return _towerAreaRect == null ? new Vector2(localX, localY) : new Vector2(localX, localY);
        }

        public float AnchoredXToLocalX(float anchoredX) =>
            anchoredX;

        public bool ScreenPointToLocalX(Vector2 screenPosition, out float localX)
        {
            localX = 0f;
            if (_towerAreaRect == null)
                return false;
            
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_towerAreaRect, screenPosition, _camera, out Vector2 localPoint))
                return false;
            
            localX = localPoint.x;
            
            return true;
        }

        public bool ScreenPointToLocalPoint(Vector2 screenPosition, out Vector2 localPoint)
        {
            localPoint = Vector2.zero;
            return _towerAreaRect != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(_towerAreaRect, screenPosition, _camera, out localPoint);
        }
    }
}
