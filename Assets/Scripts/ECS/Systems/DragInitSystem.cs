using Leopotam.EcsProto;

namespace ECS.Systems
{
    public sealed class DragInitSystem : IProtoInitSystem
    {
        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            var aspect = (GameAspect)world.Aspect(typeof(GameAspect));

            ref var state = ref aspect.DragStatePool.NewEntity(out var dragEntity);
            state.OriginalEntity = default;
            state.WasInScroll = false;
            state.PendingDrop = false;

            ref var drag = ref aspect.DragPool.Add(dragEntity);
            drag.IsDragging = false;
            drag.PointerWorldPosition = default;
            drag.PointerScreenPosition = default;

            ref var pos = ref aspect.PositionPool.Add(dragEntity);
            pos.Value = default;

            aspect.DragEntity = dragEntity;
        }
    }
}
