using System;
using System.Drawing;

namespace FontAwesome.Sharp
{
    internal static class FormsIconExtensions
    {
        public static void Rotate(this Graphics graphics, double rotation, int width, int height)
        {
            if (Math.Abs(rotation) < 0.5) return;
            float mx = .5f * width, my = .5f * height;
            graphics.TranslateTransform(mx, my);
            graphics.RotateTransform((float)rotation);
            graphics.TranslateTransform(-mx, -my);
        }

        public static void Flip(this Graphics graphics, FlipOrientation flip, int width, int height)
        {
            switch (flip)
            {
                case FlipOrientation.Horizontal:
                    graphics.ScaleTransform(-1f, 1f);
                    graphics.TranslateTransform(-width, 0f);
                    break;

                case FlipOrientation.Vertical:
                    graphics.ScaleTransform(1f, -1f);
                    graphics.TranslateTransform(0f, -height);
                    break;
            }
        }

        public static void Flip(this Image image, FlipOrientation flip)
        {
            var rotateFlip = flip.ToRotateFlip();
            if (rotateFlip == RotateFlipType.RotateNoneFlipNone) return;
            image.RotateFlip(rotateFlip);
        }

        private static RotateFlipType ToRotateFlip(this FlipOrientation flip)
        {
            switch (flip)
            {
                case FlipOrientation.Horizontal: return RotateFlipType.RotateNoneFlipX;
                case FlipOrientation.Vertical: return RotateFlipType.RotateNoneFlipY;
                default: return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }
}
