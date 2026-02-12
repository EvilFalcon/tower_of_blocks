namespace ECS.Components
{
    /// <summary>
    /// Marks that a block entity is currently part of the tower.
    /// </summary>
    public struct TowerBlockComponent
    {
        /// <summary>
        /// Index of this block in the tower (0 = bottom).
        /// </summary>
        public int TowerIndex;
    }
}

