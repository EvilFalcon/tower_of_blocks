using System;
using System.Collections.Generic;
using ECS;
using Leopotam.EcsProto;
using MVP.View;
using Services;
using Configuration;
using UI;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVP.Presenter
{
    public sealed class BlocksScrollPresenter
    {
        private readonly BlocksScrollView _view;
        private readonly GameAspect _aspect;
        private readonly IGameConfigProvider _configProvider;
        private readonly IBlockDragService _dragService;
        private readonly BlockView _blockPrefab;
        private readonly Canvas _canvas;
        private readonly Camera _camera;

        private readonly Dictionary<ProtoEntity, BlockPresenter> _presenters = new();
        private readonly HashSet<ProtoEntity> _hiddenViews = new HashSet<ProtoEntity>();
        private CompositeDisposable _disposables = new();

        public BlocksScrollPresenter(
            BlocksScrollView view,
            GameAspect aspect,
            IGameConfigProvider configProvider,
            IBlockDragService dragService,
            BlockView blockPrefab,
            Canvas canvas,
            Camera camera)
        {
            _view = view;
            _aspect = aspect;
            _configProvider = configProvider;
            _dragService = dragService;
            _blockPrefab = blockPrefab;
            _canvas = canvas;
            _camera = camera;
        }

        public void Initialize()
        {
            Transform content = _view.ScrollRect.content;

            var existingBlocksCount = 0;
            
            foreach (var entity in _aspect.BlockIt)
            {
                if (_aspect.BlockPool.Has(entity))
                {
                    existingBlocksCount++;
                }
            }
            
            var createdBlocksCount = 0;
            
            for (var i = 0; i < _configProvider.BlocksCount; i++)
            {
                ref var block = ref _aspect.BlockPool.NewEntity(out var entity);
                block.Id = i;
                block.IsInScroll = true;

                CreateBlockView(entity, content, i);
                createdBlocksCount++;
            }

            Observable.EveryUpdate()
                .Subscribe(_ => RemovePresentersForBlocksLeftScroll())
                .AddTo(_disposables);
        }

        private void RemovePresentersForBlocksLeftScroll()
        {
            var toRemove = new List<ProtoEntity>();

            foreach (var kvp in _presenters)
            {
                try
                {
                    if (!_aspect.BlockPool.Has(kvp.Key))
                    {
                        toRemove.Add(kvp.Key);
                    }
                    else
                    {
                        ref var block = ref _aspect.BlockPool.Get(kvp.Key);

                        if (block.IsInScroll || _hiddenViews.Contains(kvp.Key))
                            continue;

                        toRemove.Add(kvp.Key);
                        _hiddenViews.Add(kvp.Key);
                    }
                }
                catch (Exception)
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var entity in toRemove)
            {
                if (!_presenters.TryGetValue(entity, out var presenter))
                    continue;

                var view = presenter.GetView();
               
                if (view is BlockView blockView)
                {
                    try
                    {
                        if (blockView.EntityRef != null)
                            blockView.EntityRef.Entity = default;
                    }
                    catch (MissingReferenceException)
                    {
                    }
                }
                    
                presenter.Dispose();
                _presenters.Remove(entity);
                _hiddenViews.Remove(entity);
            }
        }

        private void CreateBlockView(ProtoEntity entity, Transform parent, int blockIndex)
        {
            var blockView = Object.Instantiate(_blockPrefab, parent);
            var blockObj = blockView.gameObject;

            blockObj.SetActive(true);

            var entityRef = blockView.EntityRef;
           
            entityRef.Entity = entity;

            var dragHandler = blockView.DragHandler;
            
            dragHandler.Initialize(_dragService, _canvas, _camera);
            dragHandler.SetScrollRect(_view.ScrollRect);

            var sprite = _configProvider.GetSprite(blockIndex);

            if (sprite != null)
                blockView.SetImage(sprite);

            blockView.SetAnimationConfig(_configProvider.Animations);

            var presenter = new BlockPresenter(blockView, entity, _aspect);
            presenter.Initialize();
            
            _presenters[entity] = presenter;
        }
    }
}