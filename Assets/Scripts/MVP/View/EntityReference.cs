using Leopotam.EcsProto;

namespace MVP.View
{
    /// <summary>
    /// Component that stores reference to ECS entity for View components.
    /// </summary>
    public sealed class EntityReference : UnityEngine.MonoBehaviour
    {
        public ProtoEntity Entity { get; set; }
    }
}