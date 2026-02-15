using ECS.Components;
using Leopotam.EcsProto;

namespace ECS
{
    public sealed class GameAspect : IProtoAspect
    {
        private ProtoWorld _world;

        public ProtoPool<BlockComponent> BlockPool { get; private set; }
        public ProtoPool<TowerBlockComponent> TowerBlockPool { get; private set; }
        public ProtoPool<PositionComponent> PositionPool { get; private set; }
        public ProtoPool<ColorComponent> ColorPool { get; private set; }
        public ProtoPool<DragComponent> DragPool { get; private set; }
        public ProtoPool<DragStateComponent> DragStatePool { get; private set; }
        public ProtoPool<AnimationComponent> AnimationPool { get; private set; }
        public ProtoPool<PooledBlockComponent> PooledBlockPool { get; private set; }

        public ProtoIt BlockIt { get; private set; }
        public ProtoIt DragIt { get; private set; }
        public ProtoIt DragStateIt { get; private set; }
        public ProtoIt TowerIt { get; private set; }

        public ProtoEntity DragEntity { get; set; }

        public void Init(ProtoWorld world)
        {
            _world = world;
            _world.AddAspect(this);

            BlockPool = new ProtoPool<BlockComponent>();
            _world.AddPool(BlockPool);

            TowerBlockPool = new ProtoPool<TowerBlockComponent>();
            _world.AddPool(TowerBlockPool);

            PositionPool = new ProtoPool<PositionComponent>();
            _world.AddPool(PositionPool);

            ColorPool = new ProtoPool<ColorComponent>();
            _world.AddPool(ColorPool);

            DragPool = new ProtoPool<DragComponent>();
            _world.AddPool(DragPool);

            DragStatePool = new ProtoPool<DragStateComponent>();
            _world.AddPool(DragStatePool);

            AnimationPool = new ProtoPool<AnimationComponent>();
            _world.AddPool(AnimationPool);

            PooledBlockPool = new ProtoPool<PooledBlockComponent>();
            _world.AddPool(PooledBlockPool);
        }

        public void PostInit()
        {
            BlockIt = new ProtoIt(new[] { typeof(BlockComponent) });
            BlockIt.Init(_world);

            DragIt = new ProtoIt(new[] { typeof(DragComponent) });
            DragIt.Init(_world);

            DragStateIt = new ProtoIt(new[] { typeof(DragStateComponent) });
            DragStateIt.Init(_world);

            TowerIt = new ProtoIt(new[] { typeof(TowerBlockComponent) });
            TowerIt.Init(_world);
        }

        public ProtoWorld World() => _world;
    }
}
