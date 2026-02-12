using UnityEngine;

namespace Configuration
{
    /// <summary>
    /// Tower area bounds configuration (margins, offsets).
    /// Actual bounds are calculated from UI RectTransform at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "TowerBoundsConfig", menuName = "TowerOfBlocks/TowerBoundsConfig")]
    public sealed class TowerBoundsConfig : ScriptableObject
    {
        [SerializeField] private float _horizontalMarginPercent = 0.2f;
        [SerializeField] private float _bottomMarginPercent = 0f;
        [SerializeField] private float _topMarginPercent = 0f;

        public float HorizontalMarginPercent => _horizontalMarginPercent;
        public float BottomMarginPercent => _bottomMarginPercent;
        public float TopMarginPercent => _topMarginPercent;
    }
}
