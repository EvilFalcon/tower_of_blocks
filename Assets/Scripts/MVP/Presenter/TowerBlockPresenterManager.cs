using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using ECS;
using ECS.Components;
using Leopotam.EcsProto;
using MVP.Interfaces;
using MVP.View;
using R3;
using Services;
using UI;
using UnityEngine;
using Utils;

namespace MVP.Presenter
{
    public sealed class TowerBlockPresenterManager : IDisposable

    {
        private readonly GameAspect _aspect;
        private readonly Transform _towerParent;
        private readonly IBlockViewPool _viewPool;
        private readonly IGameConfigProvider _configProvider;
        private readonly IBlockDragService _dragService;
        private readonly Canvas _canvas;
        private readonly Camera _camera;
        private readonly Dictionary<ProtoEntity, TowerBlockPresenter> _presenters = new();
        private CompositeDisposable _disposables = new();

        public TowerBlockPresenterManager(
            GameAspect aspect,
            Transform towerParent,
            IBlockViewPool viewPool,
            IGameConfigProvider configProvider,
            IBlockDragService dragService,
            Canvas canvas,
            Camera camera)
        {
            _aspect = aspect;
            _towerParent = towerParent;
            _viewPool = viewPool;
            _configProvider = configProvider;
            _dragService = dragService;
            _canvas = canvas;
            _camera = camera;
        }

        public bool HasPresenter(ProtoEntity entity) =>
            _presenters.ContainsKey(entity);

        public ITowerBlockView GetView(ProtoEntity entity) =>
            _presenters.TryGetValue(entity, out var presenter) ? presenter.GetView() : null;

        public void RemovePresenter(ProtoEntity entity)
        {
            if (!_presenters.TryGetValue(entity, out var presenter))
                return;

            presenter.Dispose();
            _presenters.Remove(entity);
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            foreach (var presenter in _presenters.Values)
            {
                presenter.Dispose();
            }

            _presenters.Clear();
            _disposables?.Dispose();
        }

        private void Update()
        {
            foreach (var entity in _aspect.TowerIt)
            {
                if (_aspect.TowerBlockPool.Has(entity) && !_presenters.ContainsKey(entity))
                {
                    CreateTowerBlockPresenter(entity);
                }
            }

            var toRemove = new List<ProtoEntity>();

            foreach (var kvp in _presenters.Where(kvp => !_aspect.TowerBlockPool.Has(kvp.Key)))
            {
                if (_aspect.AnimationPool.Has(kvp.Key))
                {
                    ref var anim = ref _aspect.AnimationPool.Get(kvp.Key);

                    if (anim.Type == AnimationType.HoleFall)
                    {
                        continue;
                    }
                }

                toRemove.Add(kvp.Key);
            }

            foreach (var entity in toRemove)
            {
                var presenter = _presenters[entity];
                var towerView = presenter.GetView();

                if (towerView is TowerBlockView tv && tv.BlockView != null)
                {
                    var bv = tv.BlockView;
                    bv.ResetState();
                    _viewPool.Return(bv);
                }

                presenter.Dispose();
                _presenters.Remove(entity);
            }
        }

        private void CreateTowerBlockPresenter(ProtoEntity entity)
        {
            var blockView = _viewPool.Get();
            blockView.transform.SetParent(_towerParent);
            var blockImage = blockView.Image;

            blockImage?.RestoreFullAlphaAndRaycast();

            var blockObj = blockView.gameObject;
            var entityRef = blockView.EntityRef;
            var towerView = blockObj.GetComponent<TowerBlockView>();

            if (towerView == null)
            {
                towerView = blockObj.AddComponent<TowerBlockView>();
            }

            entityRef.Entity = entity;

            var dragHandler = blockView.DragHandler;

            if (dragHandler != null && _dragService != null)
                dragHandler.Initialize(_dragService, _canvas, _camera);

            if (_aspect.BlockPool.Has(entity))
            {
                ref var block = ref _aspect.BlockPool.Get(entity);

                if (blockView != null && _aspect.PositionPool.Has(entity))
                {
                    var sprite = GetSpriteForBlock(block.Id);

                    if (sprite != null)
                    {
                        blockView.SetImage(sprite);
                    }
                }
            }

            blockView.SetAnimationConfig(_configProvider.Animations);
            towerView.SetAnimationConfig(_configProvider.Animations);

            var presenter = new TowerBlockPresenter(towerView, entity, _aspect);
            presenter.Initialize();
            _presenters[entity] = presenter;

            if (!_aspect.PositionPool.Has(entity))
                return;

            ref var pos = ref _aspect.PositionPool.Get(entity);
            towerView.SetPosition(pos.Value);
        }

        private Sprite GetSpriteForBlock(int blockId)
        {
            return _configProvider.GetSprite(blockId);
        }
    }
}