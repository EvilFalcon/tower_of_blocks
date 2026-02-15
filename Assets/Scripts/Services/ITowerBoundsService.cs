using UnityEngine;

namespace Services
{
    public interface ITowerBoundsService
    {
        float TopY { get; }
        float BottomY { get; }
        float LeftX { get; }
        float RightX { get; }
        float BlockHeight { get; }
        float HalfBlockHeight { get; }
        bool IsPointInside(Vector2 screenPosition);
        bool IsPointInsideWithMargin(Vector2 screenPosition, float marginInLocalUnits);
        Vector2 LocalToAnchoredPosition(float localX, float localY);
        float AnchoredXToLocalX(float anchoredX);
        bool ScreenPointToLocalX(Vector2 screenPosition, out float localX);
        bool ScreenPointToLocalPoint(Vector2 screenPosition, out Vector2 localPoint);
    }
}