using Configuration;
using ECS.Components;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Interfaces
{
    public interface ITowerBlockView
    {
        Image Image { get; }
        void SetAnimationConfig(AnimationSettings config);
        void SetPosition(Vector2 position);
        void SetActive(bool isActive);
        void PlayAnimation(AnimationType type, Vector2 targetPosition);
    }
}

