using System;
using System.Collections.Generic;
using Leopotam.EcsProto;
using MVP.View;
using UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public sealed class BlockViewPool : IBlockViewPool
    {
        private readonly BlockView _prefab;
        private readonly Transform _poolParent;
        private readonly Queue<BlockView> _available = new Queue<BlockView>();
        private readonly HashSet<BlockView> _allInstances = new HashSet<BlockView>();

        public BlockViewPool(BlockView prefab, Transform poolParent = null)
        {
            _prefab = prefab;
            _poolParent = poolParent;
        }

        public BlockView Get()
        {
            BlockView blockView;
            
            if (_available.Count > 0)
                blockView = _available.Dequeue();
            else
            {
                blockView = UnityEngine.Object.Instantiate(_prefab, _poolParent);
                _allInstances.Add(blockView);
            }
            
            blockView.gameObject.SetActive(true);
            
            var image = blockView?.Image;

            if (image == null)
                return blockView;

            var color = image.color;
            color.a = 1f;
            image.color = color;
            image.raycastTarget = true;
            return blockView;
        }

        public void Return(BlockView blockView)
        {
            if (blockView == null)
                return;

            blockView.ResetState();
            var towerView = blockView.GetComponent<MVP.View.TowerBlockView>();
            if (towerView != null)
                towerView.CancelAllMotions();

            if (!_allInstances.Contains(blockView))
                _allInstances.Add(blockView);

            blockView.gameObject.SetActive(false);
            blockView.transform.SetParent(_poolParent);

            var rect = blockView.transform as RectTransform;
            
            if (rect != null)
                rect.anchoredPosition = Vector2.zero;
            else
                blockView.transform.position = Vector3.zero;

            if (blockView.EntityRef != null)
                blockView.EntityRef.Entity = default(ProtoEntity);

            var image = blockView.Image;
            
            if (image != null)
            {
                var color = image.color;
                color.a = 1f;
                image.color = color;
                image.raycastTarget = true;
            }

            _available.Enqueue(blockView);
        }

        public void WarmUp(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var blockView = Object.Instantiate(_prefab, _poolParent);
                blockView.gameObject.SetActive(false);
                _allInstances.Add(blockView);
                _available.Enqueue(blockView);
            }
        }
    }
}
