using System;
using Leopotam.EcsProto;
using MVP.View;
using UnityEngine;

namespace Services
{
    public interface IBlockDragService
    {
        (ProtoEntity pooledEntity, BlockView pooledView)? OnDragStarted(ProtoEntity entity, Vector2 screenPosition, RectTransform parentRect, Vector2 blockCurrentPosition, Vector2 clickOffset, Sprite sprite, Transform parent);
        void OnDrag(Vector2 screenPosition, RectTransform blockRect, RectTransform parentRect, ProtoEntity? entityForImmediateApply = null, Action<Vector2> applyPositionImmediate = null);
        void OnDragEnded(Vector2 screenPosition, RectTransform parentRect);
        bool IsTowerBlock(ProtoEntity entity);
    }
}
