using Leopotam.EcsProto;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// Service for handling block drag events and converting them to ECS actions.
    /// </summary>
    public interface IBlockDragService
    {
        void OnDragStarted(ProtoEntity entity, Vector2 screenPosition);
        void OnDrag(Vector2 screenPosition);
        void OnDragEnded(Vector2 screenPosition);
    }
}
