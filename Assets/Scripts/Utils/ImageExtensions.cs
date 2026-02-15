using UnityEngine.UI;

namespace Utils
{
    public static class ImageExtensions
    {
        public static void RestoreFullAlphaAndRaycast(this Image image)
        {
            if (image == null) return;

            var c = image.color;
            c.a = 1f;
            image.color = c;
            image.raycastTarget = true;
        }

        public static void SetAlpha(this Image image, float alpha)
        {
            if (image == null) return;

            var c = image.color;
            c.a = alpha;
            image.color = c;
        }
    }
}