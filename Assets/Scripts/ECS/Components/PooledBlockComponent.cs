using Leopotam.EcsProto;
using UnityEngine;

namespace ECS.Components
{
    public struct PooledBlockComponent
    {
        public ProtoEntity OriginalEntity;
        public Vector2 OriginalPosition;
        public bool WasInScroll;
    }
}
