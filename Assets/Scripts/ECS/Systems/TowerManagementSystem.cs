using Leopotam.EcsProto;

namespace ECS.Systems
{
    /// <summary>
    /// Maintains tower structure.
    /// </summary>
    public sealed class TowerManagementSystem : IProtoInitSystem, IProtoRunSystem
    {
        private GameAspect _aspect;

        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _aspect = (GameAspect)world.Aspect(typeof(GameAspect));
        }

        public void Run()
        {
            foreach (ProtoEntity entity in _aspect.TowerIt)
            {
                ref var towerBlock = ref _aspect.TowerBlockPool.Get(entity);

                if (_aspect.PositionPool.Has(entity))
                {
                    ref var pos = ref _aspect.PositionPool.Get(entity);
                }
            }
        }
    }
}