using System;
using ECS.Components;
using ECS;
using Leopotam.EcsProto;
using MVP.Interfaces;
using MVP.View;
using R3;
using UnityEngine;
using Utils;

namespace MVP.Presenter
{
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
            try
            {
                if (_aspect.DragPool.Has(_entity))
                {
                    ref var drag = ref _aspect.DragPool.Get(_entity);

                    if (drag.IsDragging && _aspect.AnimationPool.Has(_entity))
                    {
                        ref var anim = ref _aspect.AnimationPool.Get(_entity);

                        if (anim.Type != AnimationType.None)
                        {
                            _aspect.AnimationPool.Del(_entity);
                            _lastAnimationType = AnimationType.None;
                        }
                    }
                }

                if (_aspect.PositionPool.Has(_entity))
                {
                    ref var pos = ref _aspect.PositionPool.Get(_entity);

                    if (Vector2.Distance(_lastPosition, pos.Value) > FloatConstants.PositionEpsilon)
                    {
                        _lastPosition = pos.Value;

                        if (_aspect.DragPool.Has(_entity))
                        {
                            ref var drag = ref _aspect.DragPool.Get(_entity);

                            if (drag.IsDragging)
                            {
                                _view.SetPosition(pos.Value);
                                return;
                            }
                        }

                        if (_aspect.AnimationPool.Has(_entity))
                        {
                            ref var anim = ref _aspect.AnimationPool.Get(_entity);

                            if (anim.Type == AnimationType.CollapseDown)
                            {
                                if (_lastAnimationType != anim.Type)
                                    _lastAnimationType = anim.Type;

                                _view.PlayAnimation(anim.Type, pos.Value);
                            }
                            else if (anim.Type != _lastAnimationType)
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
                        var targetPos = anim.Type == AnimationType.HoleFall
                            ? anim.TargetPosition
                            : _aspect.PositionPool.Has(_entity)
                                ? _aspect.PositionPool.Get(_entity).Value
                                : _lastPosition;

                        if (anim.Type != AnimationType.HoleFall && _aspect.PositionPool.Has(_entity))
                        {
                            ref var pos = ref _aspect.PositionPool.Get(_entity);
                            if (Vector2.Distance(_lastPosition, pos.Value) > FloatConstants.PositionEpsilon)
                                return;
                        }

                        if (anim.Type == AnimationType.HoleFall && _view is TowerBlockView tv && tv.BlockView != null)
                            tv.BlockView.ResetState();
                        _view.PlayAnimation(anim.Type, targetPos);
                    }
                }
                else if (_lastAnimationType != AnimationType.None)
                {
                    _lastAnimationType = AnimationType.None;
                }

                if (_aspect.TowerBlockPool.Has(_entity))
                    return;

                var hasHoleFall = _aspect.AnimationPool.Has(_entity) && _aspect.AnimationPool.Get(_entity).Type == AnimationType.HoleFall;
                
                if (!hasHoleFall)
                    _view.SetActive(false);
            }
            catch (Exception)
            {
                return;
            }
        }

        public ITowerBlockView GetView()
        {
            return _view;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}