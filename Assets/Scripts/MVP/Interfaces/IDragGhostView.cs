using System;
using UnityEngine;

namespace MVP.Interfaces
{
    public interface IDragGhostView
    {
        void Show(Sprite sprite, Vector2 position, float alpha = 0.6f);
        void Hide();
        void HideWithAnimation(float fadeDuration, Action onComplete = null);
        void SetPosition(Vector2 position);
        void SetAlpha(float alpha);
        RectTransform RectTransform { get; }
    }
}
