using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Packaging;

namespace FontAwesome.Sharp
{
    public static class FormsIconHelper
    {
        public static Bitmap ToBitmap(this IconChar icon, int size, Color color)
        {
            var bitmap = new Bitmap(size, size);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var text = char.ConvertFromUtf32((int)icon);
                var font = GetAdjustedIconFont(graphics, text, size, size, 4, true);
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

        internal static Font GetAdjustedIconFont(this Graphics g, string graphicString, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            for (double adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize = adjustedSize - 0.5)
            {
                var testFont = GetIconFont((float)adjustedSize);
                // Test the string with the new size
                var adjustedSizeNew = g.MeasureString(graphicString, testFont);
                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
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


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

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
    }
}