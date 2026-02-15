using ECS.Components;
using Leopotam.EcsProto;
using MVP.View;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    public interface IBlockPoolService
    {
        (ProtoEntity pooledEntity, BlockView pooledView) CreatePooledBlock(
            ProtoEntity originalScrollEntity,
            Vector2 originalPosition,
            Sprite sprite,
            Transform parent,
            IBlockDragService dragService,
            ScrollRect scrollRect = null,
            bool createView = true);

        void ReturnPooledBlock(ProtoEntity pooledEntity, BlockView pooledView);
    }
}
