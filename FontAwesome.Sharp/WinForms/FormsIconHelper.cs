using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace FontAwesome.Sharp
{
    public static class FormsIconHelper
    {
        private static readonly PrivateFontCollection Fonts = InitializeFonts();
        private static readonly FontFamily FallbackFont = Fonts.Families[0];

        public static Bitmap ToBitmap<TEnum>(this FontFamily fontFamily, TEnum icon, int width, int height, Color color,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var text = icon.ToChar();
                var font = graphics.GetAdjustedIconFont(fontFamily, text, new SizeF(width, height));
                graphics.Rotate(rotation, width, height);
                var brush = new SolidBrush(color);
                DrawIcon(graphics, font, text, width, height, brush);
            }

            bitmap.Flip(flip);
            return bitmap;
        }

        public static Bitmap ToBitmap<TEnum>(this FontFamily fontFamily, TEnum icon, int size, Color color,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return ToBitmap(fontFamily, icon, size, size, color, rotation, flip);
        }

        /// <summary>
        ///     Convert icon to bitmap image with GDI+ API - positioning of icon isn't perfect, but aliasing is good. Good for
        ///     small icons.
        /// </summary>
        public static Bitmap ToBitmap(this IconChar icon, Color color,
            int size = IconHelper.DefaultSize, double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
        {
            var fontFamily = FontFamilyFor(icon);
            return fontFamily.ToBitmap(icon, size, size, color, rotation, flip);
        }

        /// <summary>
        ///     Convert icon to bitmap image with GDI+ API - positioning of icon isn't perfect, but aliasing is good. Good for
        ///     small icons.
        /// </summary>
        public static Bitmap ToBitmap(this IconChar icon, int width, int height, Color color,
            double rotation = 0.0, FlipOrientation flip = FlipOrientation.Normal)
        {
            var fontFamily = FontFamilyFor(icon);
            return fontFamily.ToBitmap(icon, width, height, color, rotation, flip);
        }

        public static void DrawIcon(this Graphics graphics, Font font, string text, int width, int height, Brush brush)
        {
            // Set best quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PageUnit = GraphicsUnit.Pixel;

            var topLeft = graphics.GetTopLeft(text, font, new SizeF(width, height));

            graphics.DrawString(text, font, brush, topLeft);
        }

        public static void AddIcon(this ImageList imageList, IconChar icon, Color color, int size = IconHelper.DefaultSize)
        {
            imageList.Images.Add(icon.ToString(), icon.ToBitmap(color, size));
        }

        public static void AddIcons(this ImageList imageList, Color color, int size = IconHelper.DefaultSize, params IconChar[] icons)
        {
            foreach (var icon in icons)
                imageList.AddIcon(icon, color, size);
        }

        private static PointF GetTopLeft(this Graphics graphics, string text, Font font, SizeF size)
        {
            // cf.: https://www.codeproject.com/Articles/2118/Bypass-Graphics-MeasureString-limitations
            // 1.
            var iconSize = graphics.GetIconSize(text, font, size);

            // 2.
            //var rect = new RectangleF(0, 0, size.Width, size.Height);
            //var regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            //rect = regions[0].GetBounds(graphics);
            ////return new PointF(rect.Left, rect.Top);
            //iconSize = new SizeF(rect.Right, rect.Bottom);

            // 3.
            //iconSize = TextRenderer.MeasureText(text, font);

            // center icon
            var left = Math.Max(0f, (size.Width - iconSize.Width) / 2);
            var top = Math.Max(0f, (size.Height - iconSize.Height) / 2);
            return new PointF(left, top);
        }

        private static SizeF GetIconSize(this Graphics graphics, string text, Font font, SizeF size)
        {
            var format = new StringFormat();
            var ranges = new[] {new CharacterRange(0, text.Length)};
            format.SetMeasurableCharacterRanges(ranges);
            format.Alignment = StringAlignment.Center;
            var iconSize = graphics.MeasureString(text, font, size, format);
            return iconSize;
        }

        private static Font GetAdjustedIconFont(this Graphics g, FontFamily fontFamily, string text,
            SizeF size, int maxFontSize = 0, int minFontSize = 4, bool smallestOnFail = true)
        {
            var safeMaxFontSize = maxFontSize > 0 ? maxFontSize : size.Height;
            for (double adjustedSize = safeMaxFontSize; adjustedSize >= minFontSize; adjustedSize -= 0.5)
            {
                var font = GetIconFont(fontFamily, (float)adjustedSize);
                // Test the string with the new size
                var iconSize = g.GetIconSize(text, font, size);
                if (iconSize.Width < size.Width && iconSize.Height < size.Height)
                    return font;
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(fontFamily, smallestOnFail ? minFontSize : maxFontSize);
        }

        internal static FontFamily FontFamilyFor(this IconChar iconChar)
        {
            if (Fonts == null) throw new InvalidOperationException("FontAwesome source font files not found!");
            var name = IconHelper.FontFor(iconChar)?.Source;
            if (name == null) return FallbackFont;
            return Fonts.Families.FirstOrDefault(f => name.EndsWith(f.Name, StringComparison.InvariantCultureIgnoreCase)) ?? FallbackFont;
        }

        internal static FontFamily FontFamilyFor(this IconChar iconChar, IconFont iconFont)
        {
            if (iconFont == IconFont.Auto) return FontFamilyFor(iconChar);
            var key = (int)iconFont;
            if (FontForStyle.TryGetValue(key, out var fontFamily)) return fontFamily;
            fontFamily = Fonts.Families.FirstOrDefault(f =>
                f.Name.IndexOf(iconFont.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0);
            FontForStyle.Add(key, fontFamily);
            return fontFamily;
        }
        private static readonly Dictionary<int, FontFamily> FontForStyle = new Dictionary<int, FontFamily>();

        private static Font GetIconFont(FontFamily fontFamily, float size)
        {
            return new Font(fontFamily, size, GraphicsUnit.Point);
        }

        public static FontFamily LoadResourceFont(this Assembly assembly, string path, string fontFile)
        {
            var fonts = new PrivateFontCollection();
            AddFont(fonts, fontFile, assembly, path);
            return fonts.Families[0];
        }

        public static unsafe void AddFont(this PrivateFontCollection fonts, string fontFile,
            Assembly assembly = null, string path = "fonts")
        {
            var fontBytes = GetFontBytes(fontFile, assembly, path);
            fixed (byte* pFontData = fontBytes)
            {
                fonts.AddMemoryFont((IntPtr)pFontData, fontBytes.Length);
                uint dummy = 0;
                NativeMethods.AddFontMemResourceEx((IntPtr)pFontData, (uint)fontBytes.Length, IntPtr.Zero,
                    ref dummy);
            }
        }

        private static PrivateFontCollection InitializeFonts()
        {
            var fontFiles = new[] { "fa-solid-900.ttf", "fa-regular-400.ttf", "fa-brands-400.ttf" };
            var fonts = new PrivateFontCollection();
            foreach (var fontFile in fontFiles.Reverse())
            {
                try
                {
                    AddFont(fonts, fontFile);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Could not load FontAwesome: {ex}");
                    throw;
                }
            }
            return fonts;
        }

        private static byte[] GetFontBytes(string fontFile,
            Assembly assembly = null, string path = "fonts")
        {
            var safeAssembly = assembly ?? typeof(FormsIconHelper).Assembly;
            var relativeUri = new Uri($"./{safeAssembly.GetName().Name};component/{path}/{fontFile}", UriKind.Relative);
            var uri = new Uri(IconHelper.BaseUri, relativeUri);
            var streamInfo = Application.GetResourceStream(uri);
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
        /// <param name="fontFamily">The font family</param>
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
            if (isCTransparent && isBgTransparent)
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
                    var text = icon.ToChar();
                    var font = memoryGraphics.GetAdjustedIconFont(fontFamily, text, new SizeF(size, size));

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
                using var imageGraphics = Graphics.FromImage(image);
                var imgHdc = imageGraphics.GetHdc();
                NativeMethods.BitBlt(imgHdc, 0, 0, image.Width, image.Height, memoryHdc, 0, 0, 0x00CC0020);
                imageGraphics.ReleaseHdc(imgHdc);
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
                                                        alphaReverse) // Setting alpha: from bg to icon or from icon to bg
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
