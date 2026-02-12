using Configuration;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// Implementation that calculates tower bounds from UI RectTransform with config margins.
    /// </summary>
    public sealed class TowerBoundsService : ITowerBoundsService
    {
        private readonly RectTransform _towerAreaRect;
        private readonly TowerBoundsConfig _config;
        private readonly Camera _camera;

        public TowerBoundsService(RectTransform towerAreaRect, TowerBoundsConfig config, Camera camera)
        {
            _towerAreaRect = towerAreaRect;
            _config = config;
            _camera = camera ?? Camera.main;
        }

        public float TopY
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                _towerAreaRect.GetWorldCorners(corners);
                float height = corners[1].y - corners[0].y;
                return corners[1].y - height * _config.TopMarginPercent;
            }
        }

        public float BottomY
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                _towerAreaRect.GetWorldCorners(corners);
                float height = corners[1].y - corners[0].y;
                return corners[0].y + height * _config.BottomMarginPercent;
            }
        }

        public float LeftX
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                _towerAreaRect.GetWorldCorners(corners);
                float width = corners[2].x - corners[0].x;
                return corners[0].x + width * _config.HorizontalMarginPercent;
            }
        }

        public float RightX
        {
            get
            {
                Vector3[] corners = new Vector3[4];
                _towerAreaRect.GetWorldCorners(corners);
                float width = corners[2].x - corners[0].x;
                return corners[2].x - width * _config.HorizontalMarginPercent;
            }
        }

        public bool IsPointInside(Vector2 worldPosition)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_towerAreaRect, worldPosition, _camera);
        }
    }
}
