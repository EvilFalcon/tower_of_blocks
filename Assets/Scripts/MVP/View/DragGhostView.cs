using System;
using LitMotion;
using MVP.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.View
{
    public sealed class DragGhostView : MonoBehaviour, IDragGhostView
    {
        [SerializeField] private Image _image;
        [SerializeField] private CanvasGroup _canvasGroup;

        private MotionHandle _hideMotionFade;
        private MotionHandle _hideMotionScale;

        private Image Image => _image != null ? _image : (_image = GetComponent<Image>());
        private CanvasGroup CanvasGroup => _canvasGroup != null ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>());

        private void Awake()
        {
            if (_image == null) _image = GetComponent<Image>();
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        private void OnDestroy()
        {
            _hideMotionFade.TryCancel();
            _hideMotionScale.TryCancel();
        }

        public RectTransform RectTransform => transform as RectTransform;

        public void Show(Sprite sprite, Vector2 position, float alpha = 0.6f)
        {
            _hideMotionFade.TryCancel();
            _hideMotionScale.TryCancel();

            RectTransform.localScale = Vector3.one;

            Image.sprite = sprite;
            Image.enabled = true;
            var c = Image.color;
            c.a = alpha;
            Image.color = c;
            Image.raycastTarget = false;

            CanvasGroup.alpha = alpha;
            CanvasGroup.blocksRaycasts = false;

            RectTransform.anchoredPosition = position;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _hideMotionFade.TryCancel();
            _hideMotionScale.TryCancel();

            RectTransform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

        public void HideWithAnimation(float fadeDuration, Action onComplete = null)
        {
            if (!gameObject.activeInHierarchy)
            {
                onComplete?.Invoke();
                return;
            }

            _hideMotionFade.TryCancel();
            _hideMotionScale.TryCancel();

            var duration = Mathf.Max(0.05f, fadeDuration);

            var startAlpha = CanvasGroup.alpha;
            _hideMotionFade = LMotion.Create(startAlpha, 0f, duration)
                .WithEase(Ease.OutQuad)
                .Bind(CanvasGroup, static (a, cg) => cg.alpha = a);
        }

        public void SetPosition(Vector2 position)
        {
            if (RectTransform != null)
                RectTransform.anchoredPosition = position;
        }

        public void SetAlpha(float alpha)
        {
            var c = Image.color;
            c.a = alpha;
            Image.color = c;

            CanvasGroup.alpha = alpha;
        }
    }
}