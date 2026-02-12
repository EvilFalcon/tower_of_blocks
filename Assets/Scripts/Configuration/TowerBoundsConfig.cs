using UnityEngine;

namespace Configuration
{
    /// <summary>
    /// Tower area bounds configuration.
    /// </summary>
    [CreateAssetMenu(fileName = "TowerBoundsConfig", menuName = "TowerOfBlocks/TowerBoundsConfig")]
    public sealed class TowerBoundsConfig : ScriptableObject
    {
        [SerializeField] private Vector2 topLeftCorner;
        [SerializeField] private Vector2 bottomRightCorner;

        public Vector2 TopLeftCorner => topLeftCorner;
        public Vector2 BottomRightCorner => bottomRightCorner;
        public float TopY => topLeftCorner.y;
        public float BottomY => bottomRightCorner.y;
        public float LeftX => topLeftCorner.x;
        public float RightX => bottomRightCorner.x;
    }
}
