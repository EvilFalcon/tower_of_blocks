using ECS.Components;
using LitMotion;
using LitMotion.Adapters;
using LitMotion.Extensions;
using MVP.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.View
{
    /// <summary>
    /// MonoBehaviour implementation of IBlockView.
    /// </summary>
    public sealed class BlockView : MonoBehaviour, IBlockView
    {
        [SerializeField] private Image _image;

        private MotionHandle _currentMotion;

        private void Awake()
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
        }

        private void OnDestroy()
        {
            _currentMotion.Cancel();
        }

        public void SetPosition(Vector2 position)
        {
            _currentMotion.Cancel();

            RectTransform rect = transform as RectTransform;

            if (rect != null)
            {
                rect.anchoredPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }

        public void SetImage(Sprite sprite)
        {
            if (_image != null)
            {
                _image.sprite = sprite;
            }
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void PlayAnimation(AnimationType type, Vector2 targetPosition)
        {
            _currentMotion.Cancel();

            RectTransform rect = transform as RectTransform;
            Vector2 startPosition = rect != null ? rect.anchoredPosition : (Vector2)transform.position;

            switch (type)
            {
                case AnimationType.PlaceBounce:
                    if (rect != null)
                    {
                        Vector2 bounceTarget = startPosition + Vector2.up * 20f;
                        _currentMotion = LMotion.Create(startPosition, bounceTarget, 0.15f)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                        LMotion.Create(bounceTarget, targetPosition, 0.15f)
                            .WithEase(Ease.InQuad)
                            .WithDelay(0.15f)
                            .BindToAnchoredPosition(rect);
                    }
                    break;

                case AnimationType.HoleFall:
                    if (rect != null)
                    {
                        _currentMotion = LMotion.Create(startPosition, targetPosition, 0.3f)
                            .WithEase(Ease.InQuad)
                            .BindToAnchoredPosition(rect);
                    }
                    break;

                case AnimationType.MissDisappear:
                    if (_image != null)
                    {
                        Color startColor = _image.color;
                        Color endColor = startColor;
                        endColor.a = 0f;
                        _currentMotion = LMotion.Create(startColor, endColor, 0.2f)
                            .WithEase(Ease.OutQuad)
                            .Bind(_image, static (color, img) => img.color = color);
                        LMotion.Create(0f, 0f, 0f)
                            .WithDelay(0.2f)
                            .Bind(gameObject, static (_, go) => go.SetActive(false));
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case AnimationType.CollapseDown:
                    if (rect != null)
                    {
                        _currentMotion = LMotion.Create(startPosition, targetPosition, 0.2f)
                            .WithEase(Ease.OutQuad)
                            .BindToAnchoredPosition(rect);
                    }
                    else
                    {
                        _currentMotion = LMotion.Create(startPosition, targetPosition, 0.2f)
                            .WithEase(Ease.OutQuad)
                            .BindToPositionXY(transform);
                    }
                    break;
            }
        }
    }
}