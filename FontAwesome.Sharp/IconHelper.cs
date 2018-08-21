using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    // cf.: 
    // * http://stackoverflow.com/questions/23108181/changing-font-icon-in-wpf-using-font-awesome
    // * http://www.codeproject.com/Tips/634540/Using-Font-Icons
    public static class IconHelper
    {
        public static readonly Brush DefaultBrush = SystemColors.WindowTextBrush; // this is TextBlock default brush
        public const double DefaultSize = 16.0;

        public static ImageSource ToImageSource(this IconChar iconChar,
            Brush foregroundBrush = null, double size = DefaultSize)
        {
            if (TypefaceFor(iconChar.ToChar(), out var gt, out var glyphIndex) == null)
                return null;
            var fontSize = PixelsToPoints(size);
            var width = gt.AdvanceWidths[glyphIndex];
            var glyphRun = new GlyphRun(gt, 0, false, fontSize,
                new[] { glyphIndex }, new Point(0, 0), new[] { width },
                null, null, null, null, null, null);
            var glyphRunDrawing = new GlyphRunDrawing(foregroundBrush ?? DefaultBrush, glyphRun);
            return new DrawingImage(glyphRunDrawing);
        }

        public static char ToChar(this IconChar iconChar)
        {
            return char.ConvertFromUtf32((int)iconChar).Single();
        }

        public static FontFamily FontFor(IconChar iconChar)
        {
            return TypefaceFor(iconChar.ToChar(), out _, out _)?.FontFamily;
        }

        public static FontFamily GetFont(this Assembly assembly, string path, string fontTitle)
        {
            return new FontFamily(BaseUri, $"./{assembly.GetName().Name};component/{path}/#{fontTitle}");
        }

        private static readonly Uri BaseUri = new Uri($"{System.IO.Packaging.PackUriHelper.UriSchemePack}://application:,,,/");
        private const string FontPath = "./FontAwesome.Sharp;component/fonts/";

        private static readonly string[] FontTitles =
        {
            "Font Awesome 5 Free Solid",
            //"Font Awesome 5 Free Regular",
            "Font Awesome 5 Brands Regular"
        };

        private static readonly Typeface[] Typefaces = FontTitles.Select(GetTypeFace).ToArray();
        private static readonly int Dpi = GetDpi();

        private static Typeface GetTypeFace(string fontTitle)
        {
            var fontFamily = Assembly.GetExecutingAssembly().GetFont("fonts", fontTitle);
            return new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        private static Typeface TypefaceFor(char c, out GlyphTypeface gt, out ushort glyphIndex)
        {
            gt = null;
            glyphIndex = 42;
            foreach (var typeface in Typefaces)
                if (typeface.TryGetGlyphTypeface(out gt) && gt.CharacterToGlyphMap.TryGetValue(c, out glyphIndex))
                    return typeface;
            return SystemFonts.MessageFontFamily.GetTypefaces().FirstOrDefault(); 
        }

        private static double PixelsToPoints(double size)
        {
            // pixels to points, cf.: http://stackoverflow.com/a/139712/2592915
            return size * (72.0 / Dpi);
        }

        private static int GetDpi()
        {
            // How can I get the DPI in WPF?, cf.: http://stackoverflow.com/a/12487917/2592915
            var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            return (int) dpiProperty.GetValue(null, null);
        }
    }
}
