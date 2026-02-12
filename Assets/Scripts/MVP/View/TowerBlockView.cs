using ECS.Components;
using LitMotion;
using LitMotion.Extensions;
using MVP.Interfaces;
using UnityEngine;

namespace MVP.View
{
    /// <summary>
    /// MonoBehaviour implementation of ITowerBlockView.
    /// </summary>
    public sealed class TowerBlockView : MonoBehaviour, ITowerBlockView
    {
        private MotionHandle _currentMotion;

        private void OnDestroy()
        {
            _currentMotion.Cancel();
        }

        public void SetPosition(Vector2 position)
        {
            _currentMotion.Cancel();
            transform.position = position;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void PlayAnimation(AnimationType type, Vector2 targetPosition)
        {
            _currentMotion.Cancel();

            Vector2 startPosition = transform.position;

            switch (type)
            {
                case AnimationType.PlaceBounce:
                    Vector2 bounceTarget = startPosition + Vector2.up * 0.5f;
                    _currentMotion = LMotion.Create(startPosition, bounceTarget, 0.15f)
                        .WithEase(Ease.OutQuad)
                        .BindToPositionXY(transform);
                    LMotion.Create(bounceTarget, targetPosition, 0.15f)
                        .WithEase(Ease.InQuad)
                        .WithDelay(0.15f)
                        .BindToPositionXY(transform);
                    break;

                case AnimationType.CollapseDown:
                    _currentMotion = LMotion.Create(startPosition, targetPosition, 0.2f)
                        .WithEase(Ease.OutQuad)
                        .BindToPositionXY(transform);
                    break;
            }
        }
    }
}

