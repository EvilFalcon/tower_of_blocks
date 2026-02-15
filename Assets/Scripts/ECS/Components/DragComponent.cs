using UnityEngine;

namespace ECS.Components
{
    public struct DragComponent
    {
        public bool IsDragging;

        public Vector2 PointerWorldPosition;
        public Vector2 PointerScreenPosition;
    }
}

