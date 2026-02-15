using Leopotam.EcsProto;

namespace ECS.Components
{
    public struct DragStateComponent
    {
        public ProtoEntity OriginalEntity;
        public bool WasInScroll;
        public bool PendingDrop;
    }
}
