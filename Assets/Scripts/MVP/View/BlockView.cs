using System;
using Configuration;
using ECS.Components;
using LitMotion;
using LitMotion.Extensions;
using MVP.Interfaces;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MVP.View
{
    public sealed class BlockView : MonoBehaviour, IBlockView
    {
        [SerializeField] private Image _image;
        [SerializeField] private EntityReference _entityRef;
        [SerializeField] private BlockDragHandler _dragHandler;

        private MotionHandle _currentMotion;
        private AnimationSettings _animationConfig;

        public Image Image => _image != null ? _image : (_image = GetComponent<Image>());
        public EntityReference EntityRef => _entityRef != null ? _entityRef : (_entityRef = GetComponent<EntityReference>());
        public BlockDragHandler DragHandler => _dragHandler != null ? _dragHandler : (_dragHandler = GetComponent<BlockDragHandler>());

        private void Awake()
        {
            if (_image == null) _image = GetComponent<Image>();
            if (_entityRef == null) _entityRef = GetComponent<EntityReference>();
            if (_dragHandler == null) _dragHandler = GetComponent<UI.BlockDragHandler>();
        }

        private void OnDestroy()
        {
            _currentMotion.TryCancel();
        }

        public void SetAnimationConfig(AnimationSettings config)
        {
            _animationConfig = config;
        }

        public void SetPosition(Vector2 position)
        {
            _currentMotion.TryCancel();

            RectTransform rect = transform as RectTransform;
            if (rect != null)
                rect.anchoredPosition = position;
            else
                transform.position = position;
        }

        public void SetImage(Sprite sprite)
        {
            if (Image != null)
                Image.sprite = sprite;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void PlayAnimation(AnimationType type, Vector2 targetPosition)
        {
            _currentMotion.TryCancel();

            var rect = transform as RectTransform;
            var startPosition = rect != null ? rect.anchoredPosition : (Vector2)transform.position;

            var pb = _animationConfig.PlaceBounce;
            var hf = _animationConfig.HoleFall;
            var cd = _animationConfig.CollapseDown;
            var md = _animationConfig.MissDisappear;

            switch (type)
            {
                case AnimationType.PlaceBounce:
                    if (rect != null)
                    {
                        var offset = pb.BounceOffsetPixels > 0 ? pb.BounceOffsetPixels : 20f;
                        var upDur = pb.BounceUpDuration > 0 ? pb.BounceUpDuration : 0.15f;
                        var downDur = pb.BounceDownDuration > 0 ? pb.BounceDownDuration : 0.15f;
                        var bounceTarget = startPosition + Vector2.up * offset;
                        _currentMotion = LMotion.Create(startPosition, bounceTarget, upDur)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                        LMotion.Create(bounceTarget, targetPosition, downDur)
                            .WithEase(Ease.InQuad)
                            .WithDelay(upDur)
                            .BindToAnchoredPosition(rect);
                    }

                    break;

                case AnimationType.HoleFall:
                    if (rect != null)
                    {
                        var dur = hf.ArcDuration > 0 ? hf.ArcDuration : 0.3f;
                        _currentMotion = LMotion.Create(startPosition, targetPosition, dur)
                            .WithEase(Ease.InQuad)
                            .BindToAnchoredPosition(rect);
                    }

                    break;

                case AnimationType.MissDisappear:
                    if (Image != null)
                    {
                        var fadeDur = md.FadeDuration > 0 ? md.FadeDuration : 0.2f;
                        var hideDelay = md.HideDelay > 0 ? md.HideDelay : 0.2f;
                        var startColor = Image.color;
                        var endColor = startColor;
                        endColor.a = 0f;
                        _currentMotion = LMotion.Create(startColor, endColor, fadeDur)
                            .WithEase(Ease.OutQuad)
                            .Bind(Image, static (color, img) => img.color = color);
                        LMotion.Create(0f, 0f, 0f)
                            .WithDelay(hideDelay)
                            .Bind(gameObject, static (_, go) => go.SetActive(false));
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }

                    break;

                case AnimationType.CollapseDown:
                    var collapseDur = cd.Duration > 0 ? cd.Duration : 0.2f;

                    if (rect != null)
                    {
                        _currentMotion = LMotion.Create(startPosition, targetPosition, collapseDur)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                    }
                    else
                    {
                        _currentMotion = LMotion.Create(startPosition, targetPosition, collapseDur)
                            .WithEase(Ease.OutQuad)
                            .BindToPositionXY(transform);
                    }

                    break;
                case AnimationType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void FadeOut(float duration)
        {
            _currentMotion.TryCancel();
            if (Image == null) return;

            var d = duration > 0 ? duration : (_animationConfig.MissDisappear.FadeDuration > 0 ? _animationConfig.MissDisappear.FadeDuration : 0.35f);
            var startColor = Image.color;
            var endColor = startColor;
            endColor.a = 0f;
            _currentMotion = LMotion.Create(startColor, endColor, d)
                .WithEase(Ease.OutQuad)
                .Bind(Image, static (color, img) => img.color = color);
        }

        public Vector2 GetPosition()
        {
            if (this == null || transform == null)
                return Vector2.zero;

            var rect = transform as RectTransform;
            return rect != null ? rect.anchoredPosition : transform.position;
        }

        public void ResetState()
        {
            _currentMotion.TryCancel();
            Image?.RestoreFullAlphaAndRaycast();
        }
    }
}