namespace ECS.Components
{
    /// <summary>
    /// Base data for a block entity.
    /// Pure data (no logic) as per ECS principles.
    /// </summary>
    public struct BlockComponent
    {
        /// <summary>
        /// Logical identifier of the block (index in config, etc.).
        /// </summary>
        public int Id;

        /// <summary>
        /// Whether this block currently belongs to the bottom scroll strip.
        /// If false, it is part of the tower or is being animated / removed.
        /// </summary>
        public bool IsInScroll;
    }
}

