using UnityEngine;

namespace Configuration
{
    [System.Serializable]
    public struct TowerBoundsSettings
    {
        [Tooltip("Горизонтальный отступ от краёв области башни (0..1).")]
        public float HorizontalMarginPercent;

        [Tooltip("Нижний отступ (0..1).")] public float BottomMarginPercent;
        [Tooltip("Верхний отступ (0..1).")] public float TopMarginPercent;

        [Tooltip("Высота одного блока (должна совпадать с высотой префаба блока).")]
        public float BlockHeight;
    }

    [System.Serializable]
    public struct PlaceBounceSettings
    {
        public float BounceUpDuration;
        public float BounceDownDuration;

        [Tooltip("Смещение вверх в пикселях (RectTransform).")]
        public float BounceOffsetPixels;
    }

    [System.Serializable]
    public struct HoleFallSettings
    {
        [Tooltip("Высота дуги в пикселях.")] public float ArcHeight;
        public float ArcDuration;
        public float FadeDuration;
    }

    [System.Serializable]
    public struct CollapseDownSettings
    {
        public float Duration;
    }

    [System.Serializable]
    public struct MissDisappearSettings
    {
        public float FadeDuration;
        public float HideDelay;
    }

    [System.Serializable]
    public struct DragGhostSettings
    {
        [Range(0f, 1f)] public float Alpha;
    }

    [System.Serializable]
    public struct AnimationTimingSettings
    {
        [Tooltip("Секунд после начала анимации MissDisappear/HoleFall до удаления entity.")]
        public float RemoveAfterAnimationSeconds;
    }

    [System.Serializable]
    public struct AnimationSettings
    {
        public PlaceBounceSettings PlaceBounce;
        public HoleFallSettings HoleFall;
        public CollapseDownSettings CollapseDown;
        public MissDisappearSettings MissDisappear;
        public DragGhostSettings DragGhost;
        public AnimationTimingSettings Timing;
    }
}