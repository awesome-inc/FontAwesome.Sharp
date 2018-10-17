using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace FontAwesome.Sharp
{
    public static class FormsIconHelper
    {
        private static readonly PrivateFontCollection Fonts = InitializeFonts();

        public static Bitmap ToBitmap<TEnum>(this FontFamily fontFamily, TEnum icon, int size, Color color,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var bitmap = new Bitmap(size, size);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var text = icon.ToChar().ToString();
                var font = GetAdjustedIconFont(graphics, fontFamily, text, size, size);
                graphics.Rotate(rotation, size, size);
                var brush = new SolidBrush(color);
                DrawIcon(graphics, font, text, size, size, brush);
            }

            bitmap.Flip(flip);
            return bitmap;
        }

        /// <summary>
        ///     Convert icon to bitmap image with GDI+ API - positioning of icon isn't perfect, but aliasing is good. Good for
        ///     small icons.
        /// </summary>
        public static Bitmap ToBitmap(this IconChar icon, int size, Color color,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
        {
            var fontFamily = FontFamilyFor(icon);
            return fontFamily.ToBitmap(icon, size, color, rotation, flip);
        }

        public static void DrawIcon(this Graphics graphics, Font font, string text, int width, int height, Brush brush)
        {
            // Set best quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;


            // Measure string so that we can center the icon.
            var stringSize = graphics.MeasureString(text, font, width);
            var w = stringSize.Width;
            var h = stringSize.Height;

            // center icon
            var left = (width - w) / 2;
            var top = (height - h) / 2;

            // Draw string to screen.
            graphics.DrawString(text, font, brush, new PointF(left, top));
        }

        public static void AddIcon(this ImageList imageList, IconChar icon, int size, Color color)
        {
            imageList.Images.Add(icon.ToString(), icon.ToBitmap(size, color));
        }

        public static void AddIcons(this ImageList imageList, int size, Color color, params IconChar[] icons)
        {
            foreach (var icon in icons)
                imageList.AddIcon(icon, size, color);
        }

        private static Font GetAdjustedIconFont(this Graphics g, FontFamily fontFamily, string text,
            int width, int height, int maxFontSize = 0, int minFontSize = 4, bool smallestOnFail = true)
        {
            var safeMaxFontSize = maxFontSize > 0 ? maxFontSize : height;
            for (double adjustedSize = safeMaxFontSize; adjustedSize >= minFontSize; adjustedSize = adjustedSize - 0.5)
            {
                var testFont = GetIconFont(fontFamily, (float)adjustedSize);
                // Test the string with the new size
                var adjustedSizeNew = g.MeasureString(text, testFont);
                if (width > adjustedSizeNew.Width && height > adjustedSizeNew.Height)
                    return testFont;
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(fontFamily, smallestOnFail ? minFontSize : maxFontSize);
        }

        internal static FontFamily FontFamilyFor(IconChar iconChar)
        {
            var name = IconHelper.FontFor(iconChar).FamilyNames.Values.Single();
            return Fonts.Families.FirstOrDefault(f => name.StartsWith(f.Name)) ?? Fonts.Families[0];
        }

        private static Font GetIconFont(FontFamily fontFamily, float size)
        {
            return new Font(fontFamily, size, GraphicsUnit.Point);
        }

        private static unsafe PrivateFontCollection InitializeFonts()
        {
            var fontFiles = new[] { "fa-solid-900.ttf", /*"fa-regular-400.ttf",*/ "fa-brands-400.ttf" };
            var fonts = new PrivateFontCollection();
            foreach (var fontFile in fontFiles.Reverse())
            {
                try
                {
                    var fontBytes = GetFontBytes(fontFile);
                    fixed (byte* pFontData = fontBytes)
                    {
                        fonts.AddMemoryFont((IntPtr)pFontData, fontBytes.Length);
                        uint dummy = 0;
                        NativeMethods.AddFontMemResourceEx((IntPtr)pFontData, (uint)fontBytes.Length, IntPtr.Zero,
                            ref dummy);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Could not load FontAwesome: {ex}");
                    throw;
                }
            }
            return fonts;
        }

        private static byte[] GetFontBytes(string fontFile)
        {
            var streamInfo = Application.GetResourceStream(
                //cf: http://stackoverflow.com/questions/6005398/uriformatexception-invalid-uri-invalid-port-specified
                new Uri($"{PackUriHelper.UriSchemePack}://application:,,,/FontAwesome.Sharp;component/fonts/{fontFile}",
                    UriKind.Absolute));
            // ReSharper disable once PossibleNullReferenceException
            using (streamInfo.Stream)
            {
                return ReadAllBytes(streamInfo.Stream);
            }
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            using (memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        ///     Convert icon to image. Support only 100% transparent colors (0xFFFFFFFF, Color.Transparent constant).
        ///     Use transparent color only for case with icon over some image - in all other cases
        ///     transparent color will be replaced with parent control background color.
        ///     For icon rendering is using GDI API (not GDI+) into memory buffer
        ///     with custom rendering code for transparent colors cases.
        ///     Good for big icons and when need perfect icon positioning.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <param name="size">Size in pixels</param>
        /// <param name="color">Icon color</param>
        /// <param name="bgColor">Background color</param>
        /// <returns>Image</returns>
        internal static Bitmap ToBitmapGdi<TEnum>(this FontFamily fontFamily, TEnum icon, int size, Color color, Color bgColor)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            // create the final image to render into
            var image = new Bitmap(size, size, PixelFormat.Format32bppArgb);

            var isCTransparent = color == Color.Transparent;
            var isBgTransparent = bgColor == Color.Transparent;

            // In case when icon have 2 transparent colors - just return empty transparent image
            if (isCTransparent & isBgTransparent)
            {
                image.MakeTransparent();
                return image;
            }

            // Transparent flag
            var isTransparentRendering = false;

            // create memory buffer from desktop handle that supports alpha channel
            var memoryHdc = CreateMemoryHdc(IntPtr.Zero, image.Width, image.Height, out var dib);

            Color renColor; // Icon rendering color
            Color renBgColor; // Background rendering color
            uint visibleColorRgb = 0; // Visible color variable (if transparent color is used)
            var alphaReverse = 0xFF;

            // Reasons for constant colors rendering for transparent color support:
            // TextRenderer - is old GDI API (not GDI+) and have weak support for 
            // transparent colors. Result will be with artifacts on colors blend. 
            // This is why need to render icon with 2 constant colors.

            // 100% transparent color = 0x00rrggbb, 0% transparent color = 0xFFrrggbb
            // Color.Transparent = 0xFFFFFFFF
            // 3 variants of colors select

            if (isCTransparent) // Icon is transparent
            {
                if (bgColor.GetBrightness() <= 0.5)
                {
                    renColor = Color.Black;
                    renBgColor = Color.White;
                    alphaReverse = 0xFF;
                }
                else
                {
                    renColor = Color.White;
                    renBgColor = Color.Black;
                    alphaReverse = 0;
                }
                visibleColorRgb = (uint)bgColor.ToArgb() & 0x00FFFFFF; // Save bg color as color for rendering
                isTransparentRendering = true;
            }
            else if (isBgTransparent) // Background is transparent
            {
                if (color.GetBrightness() >= 0.5)
                {
                    renColor = Color.White;
                    renBgColor = Color.Black;
                    alphaReverse = 0;
                }
                else
                {
                    renColor = Color.Black;
                    renBgColor = Color.White;
                    alphaReverse = 0xFF;
                }
                visibleColorRgb = (uint)color.ToArgb() & 0x00FFFFFF; // Save color as color for rendering
                isTransparentRendering = true;
            }
            else // No transparent color
            {
                renColor = color;
                renBgColor = bgColor;
            }

            try
            {
                // create memory buffer graphics to use for HTML rendering
                using (var memoryGraphics = Graphics.FromHdc(memoryHdc))
                {
                    // Set best quality
                    memoryGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    memoryGraphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    memoryGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    memoryGraphics.SmoothingMode = SmoothingMode.HighQuality;

                    // Getting font and icon as text
                    var text = icon.ToChar().ToString();
                    var font = GetAdjustedIconFont(memoryGraphics, fontFamily, text, size, size);

                    // must not be transparent background 
                    memoryGraphics.Clear(renBgColor);

                    // Text rendering
                    TextRenderer.DrawText(
                        memoryGraphics, text, font,
                        new Rectangle(0, 0, size, size),
                        renColor, renBgColor,
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.NoPadding
                    );
                }

                // copy from memory buffer to image
                using (var imageGraphics = Graphics.FromImage(image))
                {
                    var imgHdc = imageGraphics.GetHdc();
                    NativeMethods.BitBlt(imgHdc, 0, 0, image.Width, image.Height, memoryHdc, 0, 0, 0x00CC0020);
                    imageGraphics.ReleaseHdc(imgHdc);
                }
            }
            finally
            {
                // release memory buffer
                NativeMethods.DeleteObject(dib);
                NativeMethods.DeleteDC(memoryHdc);
            }

            // Transparent rendering
            if (isTransparentRendering)
                unsafe
                {
                    // Image prepare 
                    var imageData = image.LockBits(
                        new Rectangle(0, 0, size, size),
                        ImageLockMode.ReadWrite,
                        PixelFormat.Format32bppArgb
                    );

                    try
                    {
                        // variables init
                        var stride =
                            (uint)size; // imageData.Stride / 4 ; 4 = bytes per pixel, as result stride is equals width in pixels
                        var currentRow = (uint*)imageData.Scan0;
                        var lastRow = currentRow + size * stride;

                        // Here is 2 cycles because bmp format can contain 
                        // empty bytes in the end of pixels horizontal string
                        // and I'm not sure if bytes in memory can or not can
                        // have similar structure.

                        for (; currentRow < lastRow; currentRow += stride)
                        {
                            int x;
                            for (x = 0; x < size; x++)
                            {
                                var c = currentRow[x] &
                                        0x000000FF; // imageData.Stride / 4 ; 4 = bytes per pixel, as result stride is equals width in pixels
                                currentRow[x] = (
                                                    (uint)Math.Abs(
                                                        (int)c -
                                                        alphaReverse) // Setting alfa: from bg to icon or from icon to bg
                                                    << 24) // 0xAA -> 0xAA000000
                                                | visibleColorRgb // 0xAA000000 + 0x00RRGGBB
                                    ;
                            }
                        }
                    }
                    finally
                    {
                        image.UnlockBits(imageData);
                    }
                }
            return image;
        }

        private static IntPtr CreateMemoryHdc(IntPtr hdc, int width, int height, out IntPtr dib)
        {
            // Create a memory DC so we can work off-screen
            var memoryHdc = NativeMethods.CreateCompatibleDC(hdc);
            NativeMethods.SetBkMode(memoryHdc, 1);

            // Create a device-independent bitmap and select it into our DC
            var info = new NativeMethods.BitMapInfo();
            info.biSize = Marshal.SizeOf(info);
            info.biWidth = width;
            info.biHeight = -height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // BI_RGB
            dib = NativeMethods.CreateDIBSection(hdc, ref info, 0, out _, IntPtr.Zero, 0);
            NativeMethods.SelectObject(memoryHdc, dib);

            return memoryHdc;
        }
    }
}
