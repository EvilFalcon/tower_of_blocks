using System;
using System.Collections.Generic;
using Configuration;
using ECS;
using Leopotam.EcsProto;
using MVP.Interfaces;
using R3;

namespace MVP.Presenter
{
    public sealed class PooledBlockPresenterManager : IDisposable
    {
        private readonly GameAspect _aspect;
        private readonly IGameConfigProvider _configProvider;
        private readonly Dictionary<ProtoEntity, BlockPresenter> _presenters = new();
        private CompositeDisposable _disposables = new();

        public PooledBlockPresenterManager(GameAspect aspect, IGameConfigProvider configProvider)
        {
            _aspect = aspect;
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
                presenter.Dispose();

            _presenters.Clear();
            _disposables?.Dispose();
        }

        public void CreatePresenter(ProtoEntity entity, IBlockView view)
        {
            if (_presenters.ContainsKey(entity))
            {
                _presenters[entity].Dispose();
                _presenters.Remove(entity);
            }

            view.SetAnimationConfig(_configProvider.Animations);

            BlockPresenter presenter = new BlockPresenter(view, entity, _aspect);
            presenter.Initialize();
            _presenters[entity] = presenter;
        }

        public void RemovePresenter(ProtoEntity entity)
        {
            if (_presenters.TryGetValue(entity, out var presenter))
            {
                presenter.Dispose();
                _presenters.Remove(entity);
            }
        }

        public IBlockView GetView(ProtoEntity entity)
        {
            return _presenters.TryGetValue(entity, out var presenter) ? presenter.GetView() : null;
        }

        private void Update()
        {
            var toRemove = new List<ProtoEntity>();

            foreach (var kvp in _presenters)
            {
                if (!_aspect.BlockPool.Has(kvp.Key) || !_aspect.PooledBlockPool.Has(kvp.Key))
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var entity in toRemove)
            {
                RemovePresenter(entity);
            }
        }
    }
}
