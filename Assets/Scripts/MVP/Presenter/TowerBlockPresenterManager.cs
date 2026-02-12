using System.Collections.Generic;
using Configuration;
using ECS;
using Leopotam.EcsProto;
using MVP.View;
using R3;
using UnityEngine;

namespace MVP.Presenter
{
    /// <summary>
    /// Manages TowerBlockPresenter instances for blocks in the tower.
    /// </summary>
    public sealed class TowerBlockPresenterManager
    {
        private readonly GameAspect _aspect;
        private readonly Transform _towerParent;
        private readonly BlockView _blockPrefab;
        private readonly IGameConfigProvider _configProvider;
        private readonly Dictionary<ProtoEntity, TowerBlockPresenter> _presenters = new();
        private CompositeDisposable _disposables = new();

        public TowerBlockPresenterManager(
            GameAspect aspect,
            Transform towerParent,
            BlockView blockPrefab,
            IGameConfigProvider configProvider)
        {
            _aspect = aspect;
            _towerParent = towerParent;
            _blockPrefab = blockPrefab;
            _configProvider = configProvider;
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
            foreach (ProtoEntity entity in _aspect.TowerIt)
            {
                if (_aspect.TowerBlockPool.Has(entity) && !_presenters.ContainsKey(entity))
                {
                    CreateTowerBlockPresenter(entity);
                }
            }

            var toRemove = new List<ProtoEntity>();

            foreach (var kvp in _presenters)
            {
                if (!_aspect.TowerBlockPool.Has(kvp.Key))
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var entity in toRemove)
            {
                _presenters[entity].Dispose();
                _presenters.Remove(entity);
            }
        }

        private void CreateTowerBlockPresenter(ProtoEntity entity)
        {
            GameObject blockObj = FindBlockGameObject(entity);
            TowerBlockView towerView;

            if (blockObj != null)
            {
                BlockView blockView = blockObj.GetComponent<BlockView>();

                if (blockView != null)
                {
                    blockObj.transform.SetParent(_towerParent);
                    towerView = blockObj.GetComponent<TowerBlockView>();

                    if (towerView == null)
                    {
                        towerView = blockObj.AddComponent<TowerBlockView>();
                    }
                }
                else
                {
                    towerView = blockObj.GetComponent<TowerBlockView>();

                    if (towerView == null)
                    {
                        towerView = blockObj.AddComponent<TowerBlockView>();
                    }
                }
            }
            else
            {
                blockObj = Object.Instantiate(_blockPrefab.gameObject, _towerParent);
                towerView = blockObj.GetComponent<TowerBlockView>();

                if (towerView == null)
                {
                    towerView = blockObj.AddComponent<TowerBlockView>();
                }

                EntityReference entityRef = blockObj.GetComponent<EntityReference>();

                if (entityRef == null)
                {
                    entityRef = blockObj.AddComponent<EntityReference>();
                }

                entityRef.Entity = entity;

                if (_aspect.BlockPool.Has(entity))
                {
                    ref var block = ref _aspect.BlockPool.Get(entity);
                    BlockView blockView = blockObj.GetComponent<BlockView>();

                    if (blockView != null && _aspect.PositionPool.Has(entity))
                    {
                        Sprite sprite = GetSpriteForBlock(block.Id);

                        if (sprite != null)
                        {
                            blockView.SetImage(sprite);
                        }
                    }
                }
            }

            TowerBlockPresenter presenter = new TowerBlockPresenter(towerView, entity, _aspect);
            presenter.Initialize();
            _presenters[entity] = presenter;

            if (_aspect.PositionPool.Has(entity))
            {
                ref var pos = ref _aspect.PositionPool.Get(entity);
                towerView.SetPosition(pos.Value);
            }
        }

        private GameObject FindBlockGameObject(ProtoEntity entity)
        {
            EntityReference[] refs = Object.FindObjectsOfType<EntityReference>();

            foreach (var refComp in refs)
            {
                if (refComp.Entity.Equals(entity))
                {
                    return refComp.gameObject;
                }
            }

            return null;
        }

        private Sprite GetSpriteForBlock(int blockId)
        {
            return _configProvider.GetSprite(blockId);
        }
    }
}