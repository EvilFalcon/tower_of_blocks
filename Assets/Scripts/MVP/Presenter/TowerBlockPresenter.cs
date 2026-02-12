using ECS.Components;
using ECS;
using Leopotam.EcsProto;
using MVP.Interfaces;
using R3;
using UnityEngine;

namespace MVP.Presenter
{
    /// <summary>
    /// Presenter for a block in the tower.
    /// </summary>
    public sealed class TowerBlockPresenter
    {
        private readonly ITowerBlockView _view;
        private readonly ProtoEntity _entity;
        private readonly GameAspect _aspect;
        private CompositeDisposable _disposables = new();

        private Vector2 _lastPosition;
        private AnimationType _lastAnimationType = AnimationType.None;

        public TowerBlockPresenter(ITowerBlockView view, ProtoEntity entity, GameAspect aspect)
        {
            _view = view;
            _entity = entity;
            _aspect = aspect;
        }

        public void Initialize()
        {
            if (_aspect.PositionPool.Has(_entity))
            {
                ref var pos = ref _aspect.PositionPool.Get(_entity);
                _lastPosition = pos.Value;
                _view.SetPosition(_lastPosition);
            }

            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposables);
        }

        private void Update()
        {
            if (_aspect.PositionPool.Has(_entity))
            {
                ref var pos = ref _aspect.PositionPool.Get(_entity);

                if (Vector2.Distance(_lastPosition, pos.Value) > 0.01f)
                {
                    _lastPosition = pos.Value;

                    if (_aspect.AnimationPool.Has(_entity))
                    {
                        ref var anim = ref _aspect.AnimationPool.Get(_entity);
                        if (anim.Type != _lastAnimationType)
                        {
                            _lastAnimationType = anim.Type;
                            _view.PlayAnimation(anim.Type, pos.Value);
                        }
                    }
                    else
                    {
                        _view.SetPosition(pos.Value);
                    }
                }
            }

            if (_aspect.AnimationPool.Has(_entity))
            {
                ref var anim = ref _aspect.AnimationPool.Get(_entity);
                if (anim.Type != _lastAnimationType)
                {
                    _lastAnimationType = anim.Type;
                    if (_aspect.PositionPool.Has(_entity))
                    {
                        ref var pos = ref _aspect.PositionPool.Get(_entity);
                        _view.PlayAnimation(anim.Type, pos.Value);
                    }
                }
            }
            else if (_lastAnimationType != AnimationType.None)
            {
                _lastAnimationType = AnimationType.None;
            }

            if (!_aspect.TowerBlockPool.Has(_entity))
            {
                _view.SetActive(false);
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}

