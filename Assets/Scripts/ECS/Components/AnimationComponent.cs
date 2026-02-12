namespace ECS.Components
{
    /// <summary>
    /// Describes an animation request for an entity.
    /// Concrete animations will be executed on the View side (LitMotion),
    /// while systems operate only on this data.
    /// </summary>
    public struct AnimationComponent
    {
        public AnimationType Type;
    }

    /// <summary>
    /// High-level animation types used by gameplay.
    /// </summary>
    public enum AnimationType : byte
    {
        None = 0,
        /// <summary>Block jumps when successfully placed on the tower.</summary>
        PlaceBounce = 1,
        /// <summary>Block falls into the hole.</summary>
        HoleFall = 2,
        /// <summary>Block disappears / explodes on miss.</summary>
        MissDisappear = 3,
        /// <summary>Block smoothly moves down when blocks above are collapsing.</summary>
        CollapseDown = 4
    }
}

