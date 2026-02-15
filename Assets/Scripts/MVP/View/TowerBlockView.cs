using Configuration;
using ECS.Components;
using LitMotion;
using LitMotion.Extensions;
using MVP.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MVP.View
{
    public sealed class TowerBlockView : MonoBehaviour, ITowerBlockView
    {
        [SerializeField] private Image _image;
        [SerializeField] private BlockView _blockView;

        private MotionHandle _currentMotion;

        public Image Image => _image != null ? _image : (_image = GetComponent<Image>());
        public BlockView BlockView => _blockView != null ? _blockView : (_blockView = GetComponent<BlockView>());
        private MotionHandle _fadeMotion;
        private AnimationSettings _animationConfig;

        private const float BounceOffsetWorldFallback = 0.5f;

        private void Awake()
        {
            if (_image == null) _image = GetComponent<Image>();
            if (_blockView == null) _blockView = GetComponent<BlockView>();
        }

        private void OnDestroy()
        {
            _currentMotion.TryCancel();
            _fadeMotion.TryCancel();
        }

        public void SetAnimationConfig(AnimationSettings config)
        {
            _animationConfig = config;
        }

        public void CancelAllMotions()
        {
            _currentMotion.TryCancel();
            _fadeMotion.TryCancel();
            Image?.RestoreFullAlphaAndRaycast();
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

            switch (type)
            {
                case AnimationType.PlaceBounce:
                    var offsetPx = pb.BounceOffsetPixels > 0 ? pb.BounceOffsetPixels : 20f;
                    var upDur = pb.BounceUpDuration > 0 ? pb.BounceUpDuration : 0.15f;
                    var downDur = pb.BounceDownDuration > 0 ? pb.BounceDownDuration : 0.15f;

                    if (rect != null)
                    {
                        var bounceTarget = startPosition + Vector2.up * offsetPx;
                        _currentMotion = LMotion.Create(startPosition, bounceTarget, upDur)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                        LMotion.Create(bounceTarget, targetPosition, downDur)
                            .WithEase(Ease.InQuad)
                            .WithDelay(upDur)
                            .BindToAnchoredPosition(rect);
                    }
                    else
                    {
                        var bounceTarget = startPosition + Vector2.up * BounceOffsetWorldFallback;
                        _currentMotion = LMotion.Create(startPosition, bounceTarget, upDur)
                            .WithEase(Ease.OutQuad)
                            .BindToPositionXY(transform);
                        LMotion.Create(bounceTarget, targetPosition, downDur)
                            .WithEase(Ease.InQuad)
                            .WithDelay(upDur)
                            .BindToPositionXY(transform);
                    }

                    break;

                case AnimationType.CollapseDown:
                    var collapseDur = cd.Duration > 0 ? cd.Duration : 0.2f;
                    var collapseStart = new Vector2(targetPosition.x, startPosition.y);

                    if (rect != null)
                    {
                        _currentMotion = LMotion.Create(collapseStart, targetPosition, collapseDur)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                    }
                    else
                    {
                        _currentMotion = LMotion.Create(collapseStart, targetPosition, collapseDur)
                            .WithEase(Ease.OutQuad)
                            .BindToPositionXY(transform);
                    }

                    break;

                case AnimationType.HoleFall:
                    if (rect != null)
                    {
                        var arcH = hf.ArcHeight > 0 ? hf.ArcHeight : 120f;
                        var arcDur = hf.ArcDuration > 0 ? hf.ArcDuration : 0.4f;
                        var fadeDur = hf.FadeDuration > 0 ? hf.FadeDuration : 0.2f;
                        var halfArc = arcDur * 0.5f;
                        var arcPeak = (startPosition + targetPosition) * 0.5f + Vector2.up * arcH;
                        _currentMotion = LMotion.Create(startPosition, arcPeak, halfArc)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                        LMotion.Create(arcPeak, targetPosition, halfArc)
                            .WithEase(Ease.InQuad)
                            .WithDelay(halfArc)
                            .BindToAnchoredPosition(rect);
                        var image = Image;

                        if (image != null)
                        {
                            _fadeMotion.TryCancel();
                            var startColor = image.color;
                            var endColor = startColor;
                            endColor.a = 0f;
                            _fadeMotion = LMotion.Create(startColor, endColor, fadeDur)
                                .WithEase(Ease.OutQuad)
                                .WithDelay(arcDur)
                                .Bind(image, (c, img) => img.color = c);
                        }
                    }

                    break;
            }
        }
    }
}