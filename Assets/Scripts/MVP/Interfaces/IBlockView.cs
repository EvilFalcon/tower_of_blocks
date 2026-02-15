using Configuration;
using ECS.Components;
using UnityEngine;

namespace MVP.Interfaces
{
    public interface IBlockView
    {
        void SetAnimationConfig(AnimationSettings config);
        void SetPosition(Vector2 position);
        void SetImage(Sprite sprite);
        void SetActive(bool isActive);
        void PlayAnimation(AnimationType type, Vector2 targetPosition);
        void FadeOut(float duration);
        Vector2 GetPosition();
        void ResetState();
    }
}

