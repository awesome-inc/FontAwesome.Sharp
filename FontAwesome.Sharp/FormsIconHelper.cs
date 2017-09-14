using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public static class FormsIconHelper
    {
        /// <summary>
        /// Convert icon to bitmap image with GDI+ API - positioning of icon isn't perfect, but aliasing is good. Good for small icons.
        /// </summary>
        public static Bitmap ToBitmap(this IconChar icon, int size, Color color)
        {
            var bitmap = new Bitmap(size, size);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var text = char.ConvertFromUtf32((int)icon);
                var font = GetAdjustedIconFont(graphics, text, size, size);
                var brush = new SolidBrush(color);
                DrawIcon(graphics, font, text, size, size, brush);
            }
            return bitmap;
        }

        public static void DrawIcon(this Graphics graphics, Font font, string text, int width, int height, Brush brush)
        {
            // Set best quality
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


            // Measure string so that we can center the icon.
            var stringSize = graphics.MeasureString(text, font, width);
            var w = stringSize.Width;
            var h = stringSize.Height;

            // center icon
            var left = (width - w)/2;
            var top = (height - h)/2;

            // Draw string to screen.
            graphics.DrawString(text, font, brush, new PointF(left, top));
        }

        public static void AddIcon(this ImageList imageList, IconChar icon, int size, Color color)
        {
            imageList.Images.Add(icon.ToString(), icon.ToBitmap(size, color));
        }

        public static void AddIcons(this ImageList imageList, int size, Color color, params IconChar[] icons)
        {
            foreach(var icon in icons)
                imageList.AddIcon(icon, size, color);
        }

        internal static Font GetAdjustedIconFont(this Graphics g, string graphicString, 
            int width, int height, int maxFontSize = 0, int minFontSize = 4, bool smallestOnFail = true)
        {
            var safeMaxFontSize = maxFontSize > 0 ? maxFontSize : height;
            for (double adjustedSize = safeMaxFontSize; adjustedSize >= minFontSize; adjustedSize = adjustedSize - 0.5)
            {
                var testFont = GetIconFont((float)adjustedSize);
                // Test the string with the new size
                var adjustedSizeNew = g.MeasureString(graphicString, testFont);
                if (width > adjustedSizeNew.Width && height > adjustedSizeNew.Height)
                {
                    // Fits! return it
                    return testFont;
                }
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(smallestOnFail ? minFontSize : maxFontSize);
        }

        private static Font GetIconFont(float size)
        {
            return new Font(Fonts.Families[0], size, GraphicsUnit.Point);
        }


        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [In] ref uint pcFonts);

        private static readonly PrivateFontCollection Fonts = InitializeFonts();

        private static unsafe PrivateFontCollection InitializeFonts()
        {
            try
            {
                var fonts = new PrivateFontCollection();
                var fontBytes = GetFontBytes();
                fixed (byte* pFontData = fontBytes)
                {
                    uint dummy = 0;
                    fonts.AddMemoryFont((IntPtr)pFontData, fontBytes.Length);
                    AddFontMemResourceEx((IntPtr)pFontData, (uint)fontBytes.Length, IntPtr.Zero, ref dummy);
                }
                return fonts;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Could not load FontAwesome: {ex}");
                throw;
            }
        }

        private static byte[] GetFontBytes()
        {
            //cf: http://stackoverflow.com/questions/6005398/uriformatexception-invalid-uri-invalid-port-specified
            // NOTE: this line is important to not have the compiler optimize out the type initialization because of unused variable!
            if (string.IsNullOrWhiteSpace(PackUriHelper.UriSchemePack))
                Trace.TraceInformation("pack:// uri scheme not supported");

            var streamInfo = System.Windows.Application.GetResourceStream(
                new Uri(@"pack://application:,,,/FontAwesome.Sharp;component/fonts/fontawesome-webfont.ttf", UriKind.Absolute));
            // ReSharper disable once PossibleNullReferenceException
            using (streamInfo.Stream)
                return ReadAllBytes(streamInfo.Stream);
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream != null)
                return memoryStream.ToArray();

            using (memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Convert icon to image. Support only 100% transparent colors (0xFFFFFFFF, Color.Transparent constant). 
        /// Use transparent color only for case with icon over some image - in all other cases
        /// transparent color will be replaced with parent control background color. 
        /// For icon rendering is using GDI API (not GDI+) into memory buffer
        /// with custom rendering code for transparent colors cases. 
        /// Good for big icons and when need perfect icon positioning.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <param name="size">Size in pixels</param>
        /// <param name="color">Icon color</param>
        /// <param name="bgColor">Background color</param>
        /// <returns>Image</returns>
        public static Bitmap ToBitmap1(
            this IconChar icon, int size, Color color, Color bgColor
        )
        {
            // create the final image to render into
            var image = new Bitmap(size, size, PixelFormat.Format32bppArgb);

            bool isCTransparent = (color == Color.Transparent);
            bool isBgTransparent = (bgColor == Color.Transparent);

            // In case when icon have 2 transparent colors - just return empty transparent image
            if (isCTransparent & isBgTransparent)
            {
                image.MakeTransparent();
                return image;
            }

            // Transparent flag
            bool isTransparentRendering = false;

            // create memory buffer from desktop handle that supports alpha channel
            IntPtr dib;
            var memoryHdc = CreateMemoryHdc(IntPtr.Zero, image.Width, image.Height, out dib);

            Color renColor;             // Icon rendering color
            Color renBgColor;           // Background rendering color
            uint visibleColorRGB = 0;   // Visible color variable (if transparent color is used)
            int alfaReverse = 0xFF;

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
                    alfaReverse = 0xFF;
                }
                else
                {
                    renColor = Color.White;
                    renBgColor = Color.Black;
                    alfaReverse = 0;
                }
                visibleColorRGB = (uint)bgColor.ToArgb() & 0x00FFFFFF;  // Save bg color as color for rendering
                isTransparentRendering = true;
            }
            else if (isBgTransparent) // Background is transparent
            {
                if (color.GetBrightness() >= 0.5)
                {
                    renColor = Color.White;
                    renBgColor = Color.Black;
                    alfaReverse = 0;
                }
                else
                {
                    renColor = Color.Black;
                    renBgColor = Color.White;
                    alfaReverse = 0xFF;
                }
                visibleColorRGB = (uint)color.ToArgb() & 0x00FFFFFF;    // Save color as color for rendering
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
                    memoryGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    memoryGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    memoryGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    // Getting font and icon as text
                    var text = char.ConvertFromUtf32((int)icon);
                    var font = GetAdjustedIconFont(memoryGraphics, text, size, size);

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
                    BitBlt(imgHdc, 0, 0, image.Width, image.Height, memoryHdc, 0, 0, 0x00CC0020);
                    imageGraphics.ReleaseHdc(imgHdc);
                }
            }
            finally
            {
                // release memory buffer
                DeleteObject(dib);
                DeleteDC(memoryHdc);
            }

            // Transparent rendering
            if (isTransparentRendering)
            {
                //image.MakeTransparent(Color.White);
                // Since icon was rendered with only 2 colors (black as 0x0 and red as 0xFF000000) 
                // we have map of transparency color for icon and now need  
                // only move this one byte from color to alfa
                // Transparent rendering logic is simple:
                // - Get pixel
                // - Move current red color to alfa color
                // - Set current visible color (bg or icon color)

                // Case, when one color or two colors transparency is >0% and <100%
                // is not yet supported. This require more complicated logic for colors blending.

                // This logic is recomended on StackOverflow 
                // for pixel processing because GetPixel is slow.
                unsafe
                {
                    // Image prepare 
                    BitmapData imageData = image.LockBits(
                        new Rectangle(0, 0, size, size),
                        ImageLockMode.ReadWrite,
                        PixelFormat.Format32bppArgb
                    );

                    // variables init
                    int x = 0;
                    uint c = 0,
                        stride = (uint)size; // imageData.Stride / 4 ; 4 = bytes per pixel, as result stride is equals width in pixels
                    ;
                    uint* currentRow = (uint*)imageData.Scan0;
                    uint* lastRow = currentRow + size * stride;

                    // Here is 2 cycles because bmp format can contatin 
                    // empty bytes in the end of pixels horizontal string
                    // and I'm not sure if bytes in memory can or not can
                    // have similar structure.

                    for (; currentRow < lastRow; currentRow += stride)
                    {
                        for (x = 0; x < size; x++)
                        {
                            c = currentRow[x] & 0x000000FF; // *4
                            currentRow[x] = (
                                (uint)(Math.Abs((int)c - alfaReverse))   // Setting alfa: from bg to icon or from icon to bg
                                << 24)                                   // 0xAA -> 0xAA000000
                                | visibleColorRGB                        // 0xAA000000 + 0x00RRGGBB
                            ; // red -> alfa; + color
                            
                        };
                    }
                    image.UnlockBits(imageData);
                }
            }

            return image;
        }

        private static IntPtr CreateMemoryHdc(IntPtr hdc, int width, int height, out IntPtr dib)
        {
            // Create a memory DC so we can work off-screen
            IntPtr memoryHdc = CreateCompatibleDC(hdc);
            SetBkMode(memoryHdc, 1);

            // Create a device-independent bitmap and select it into our DC
            var info = new BitMapInfo();
            info.biSize = Marshal.SizeOf(info);
            info.biWidth = width;
            info.biHeight = -height;
            info.biPlanes = 1;
            info.biBitCount = 32;
            info.biCompression = 0; // BI_RGB
            IntPtr ppvBits;
            dib = CreateDIBSection(hdc, ref info, 0, out ppvBits, IntPtr.Zero, 0);
            SelectObject(memoryHdc, dib);

            return memoryHdc;
        }

        [DllImport("gdi32.dll")]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BitMapInfo pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiObj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BitMapInfo
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
            public byte bmiColors_rgbBlue;
            public byte bmiColors_rgbGreen;
            public byte bmiColors_rgbRed;
            public byte bmiColors_rgbReserved;
        }
        
    }
}