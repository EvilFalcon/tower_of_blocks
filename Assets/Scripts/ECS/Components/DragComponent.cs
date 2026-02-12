using UnityEngine;

namespace ECS.Components
{
    /// <summary>
    /// Drag state for an entity that is currently being dragged by the player.
    /// </summary>
    public struct DragComponent
    {
        /// <summary>
        /// Is this entity currently being dragged.
        /// </summary>
        public bool IsDragging;

        /// <summary>
        /// World position where drag started.
        /// </summary>
        public Vector2 StartPosition;

        /// <summary>
        /// Current pointer position in world space (for systems to decide placement / miss).
        /// </summary>
        public Vector2 PointerWorldPosition;
    }
}

