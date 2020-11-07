using System;
using System.Drawing;

namespace FontAwesome.Sharp
{
    internal static class FormsIconExtensions
    {
        /// <summary>
        /// Rotate a graphics around its center for the specified angle in degrees.
        /// </summary>
        /// <param name="graphics">The graphics to rotate</param>
        /// <param name="rotation">Angle of rotation in degrees</param>
        /// <param name="width">The graphics width in pixels (for center calculation)</param>
        /// <param name="height">The graphics height in pixels (for center calculation)</param>
        public static void Rotate(this Graphics graphics, double rotation, int width, int height)
        {
            if (Math.Abs(rotation) < 0.5) return;
            float mx = .5f * width, my = .5f * height;
            graphics.TranslateTransform(mx, my);
            graphics.RotateTransform((float)rotation);
            graphics.TranslateTransform(-mx, -my);
        }

        /// <summary>
        /// Flip a graphics
        /// </summary>
        /// <param name="graphics">The graphics to flip</param>
        /// <param name="flip">The flip value</param>
        /// <param name="width">The graphics width in pixels (for center calculation)</param>
        /// <param name="height">The graphics height in pixels (for center calculation)</param>
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
                case FlipOrientation.Normal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flip), flip, null);
            }
        }

        /// <summary>
        /// Flips an image
        /// </summary>
        /// <param name="image">The image to flip</param>
        /// <param name="flip">The flip value</param>
        public static void Flip(this Image image, FlipOrientation flip)
        {
            var rotateFlip = flip.ToRotateFlip();
            if (rotateFlip == RotateFlipType.RotateNoneFlipNone) return;
            image.RotateFlip(rotateFlip);
        }

        private static RotateFlipType ToRotateFlip(this FlipOrientation flip)
        {
            return flip switch
            {
                FlipOrientation.Horizontal => RotateFlipType.RotateNoneFlipX,
                FlipOrientation.Vertical => RotateFlipType.RotateNoneFlipY,
                _ => RotateFlipType.RotateNoneFlipNone
            };
        }
    }
}
