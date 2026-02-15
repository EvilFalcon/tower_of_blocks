using UnityEngine;

namespace ECS.Components
{
    public struct AnimationComponent
    {
        public AnimationType Type;
        public float RemoveAtTime;
        public Vector2 TargetPosition;
    }

    public enum AnimationType : byte
    {
        None = 0,
        PlaceBounce = 1,
        HoleFall = 2,
        MissDisappear = 3,
        CollapseDown = 4
    }
}