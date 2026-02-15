using ECS.Components;
using ECS;
using Leopotam.EcsProto;
using MVP.Interfaces;
using R3;
using UnityEngine;
using Utils;

namespace MVP.Presenter
{
    public sealed class BlockPresenter
    {
        private readonly IBlockView _view;
        private readonly ProtoEntity _entity;
        private readonly GameAspect _aspect;
        private CompositeDisposable _disposables = new();

        private Vector2 _lastPosition;
        private Vector2? _scrollPosition;
        private AnimationType _lastAnimationType = AnimationType.None;

        public BlockPresenter(IBlockView view, ProtoEntity entity, GameAspect aspect)
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
            }
            else
            {
                Vector2 actualPosition = _view.GetPosition();
                _lastPosition = actualPosition;

                ref var pos = ref _aspect.PositionPool.Add(_entity);
                pos.Value = actualPosition;
            }

            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposables);
        }

        private void Update()
        {
            if (_view == null || (_view is MonoBehaviour mb && mb == null))
            {
                return;
            }

            var isDragging = _aspect.DragPool.Has(_entity) && _aspect.DragPool.Get(_entity).IsDragging;

            if (_aspect.BlockPool.Has(_entity))
            {
                ref var block = ref _aspect.BlockPool.Get(_entity);

                switch (block.IsInScroll)
                {
                    case false or true when !_aspect.PooledBlockPool.Has(_entity):
                        return;

                    case true when !isDragging && !_aspect.PooledBlockPool.Has(_entity):
                    {
                        if (!_scrollPosition.HasValue)
                        {
                            var currentViewPos = _view.GetPosition();
                            var isValidPosition = Vector2.Distance(currentViewPos, Vector2.zero) > FloatConstants.PositionEpsilon &&
                                                  !(Mathf.Approximately(currentViewPos.x, -90f) && Mathf.Approximately(currentViewPos.y, -1350f));

                            if (isValidPosition)
                            {
                                _scrollPosition = currentViewPos;

                                if (_aspect.PositionPool.Has(_entity))
                                {
                                    ref var posEcs = ref _aspect.PositionPool.Get(_entity);
                                    posEcs.Value = currentViewPos;
                                }
                                else
                                {
                                    ref var posEcs = ref _aspect.PositionPool.Add(_entity);
                                    posEcs.Value = currentViewPos;
                                }

                                _lastPosition = currentViewPos;
                            }
                        }

                        if (_view is MonoBehaviour mbView && !mbView.gameObject.activeSelf)
                        {
                            mbView.gameObject.SetActive(true);
                        }

                        return;
                    }
                }
            }

            if (_aspect.PositionPool.Has(_entity))
            {
                ref var pos = ref _aspect.PositionPool.Get(_entity);

                if (Vector2.Distance(_lastPosition, pos.Value) > FloatConstants.PositionEpsilon)
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

                    if (anim.Type == AnimationType.MissDisappear && _aspect.PooledBlockPool.Has(_entity))
                    {
                        _view.FadeOut(0f);
                    }
                    else if (_aspect.PositionPool.Has(_entity))
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
        }

        public IBlockView GetView()
        {
            return _view;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}