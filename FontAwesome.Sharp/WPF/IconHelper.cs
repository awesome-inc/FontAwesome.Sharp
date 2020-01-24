using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

[assembly: InternalsVisibleTo("FontAwesome.Sharp.Tests")]
namespace FontAwesome.Sharp
{
    // cf.: 
    // * http://stackoverflow.com/questions/23108181/changing-font-icon-in-wpf-using-font-awesome
    // * http://www.codeproject.com/Tips/634540/Using-Font-Icons
    public static class IconHelper
    {
        internal static readonly IconChar[] Orphans = {
            IconChar.None
            // not contained in any of the ttf-fonts!
            ,IconChar.FontAwesomeLogoFull
        };

        public static readonly IconChar[] Icons = Enum.GetValues(typeof(IconChar))
            .Cast<IconChar>().Except(Orphans).ToArray();


        public static readonly Brush DefaultBrush = SystemColors.WindowTextBrush; // this is TextBlock default brush
        public const double DefaultSize = 16.0;

        public static FontFamily LoadFont(this Assembly assembly, string path, string fontTitle)
        {
            return new FontFamily(BaseUri, $"./{assembly.GetName().Name};component/{path}/#{fontTitle}");
        }

        public static ImageSource ToImageSource<TEnum>(this FontFamily fontFamily, TEnum icon,
            Brush foregroundBrush = null, double size = DefaultSize)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (fontFamily.TypefaceFor(icon.ToChar(), out var gt, out var glyphIndex) == null)
                return null;
            return ToImageSource(foregroundBrush, size, gt, glyphIndex);
        }

        public static char ToChar<TEnum>(this TEnum icon, IFormatProvider formatProvider = null) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return char.ConvertFromUtf32(icon.ToInt32(formatProvider ?? CultureInfo.InvariantCulture)).Single();
        }

        public static ImageSource ToImageSource(this IconChar iconChar,
            Brush foregroundBrush = null, double size = DefaultSize)
        {
            var typeFace = TypefaceFor(iconChar.ToChar(), out var gt, out var glyphIndex);
            return typeFace == null ? null : ToImageSource(foregroundBrush, size, gt, glyphIndex);
        }

        public static char ToChar(this IconChar iconChar)
        {
            return ToChar<IconChar>(iconChar);
        }

        private static ImageSource ToImageSource(Brush foregroundBrush, double size, GlyphTypeface gt, ushort glyphIndex)
        {
            var fontSize = PixelsToPoints(size);
            var width = gt.AdvanceWidths[glyphIndex];
            #pragma warning disable CS0618 // Deprecated constructor
            var glyphRun = new GlyphRun(gt, 0, false, fontSize,
                new[] { glyphIndex }, new Point(0, 0), new[] { width },
                null, null, null, null, null, null);
            #pragma warning restore CS0618
            var glyphRunDrawing = new GlyphRunDrawing(foregroundBrush ?? DefaultBrush, glyphRun);
            return new DrawingImage(glyphRunDrawing);
        }

        private static Typeface TypefaceFor(this FontFamily fontFamily, char c, out GlyphTypeface gt, out ushort glyphIndex)
        {
            gt = null;
            glyphIndex = 0;
            if (c == 0)
                return null;
            foreach (var typeface in fontFamily.GetTypefaces())
                if (typeface.TryGetGlyphTypeface(out gt) && gt.CharacterToGlyphMap.TryGetValue(c, out glyphIndex))
                    return typeface;
            return null;
        }

        internal static FontFamily FontFor(IconChar iconChar)
        {
            if (Orphans.Contains(iconChar)) return null;
            var typeFace = TypefaceFor(iconChar.ToChar(), out _, out _);
            return typeFace?.FontFamily;
        }

        internal static readonly Uri BaseUri = new Uri($"{System.IO.Packaging.PackUriHelper.UriSchemePack}://application:,,,/");

        private static readonly string[] FontTitles =
        {
            "Font Awesome 5 Free Solid",
            "Font Awesome 5 Free Regular",
            "Font Awesome 5 Brands Regular"
        };

        private static readonly Typeface[] Typefaces = FontTitles.Select(GetTypeFace).ToArray();
        private static readonly int Dpi = GetDpi();

        private static Typeface GetTypeFace(string fontTitle)
        {
            var fontFamily = Assembly.GetExecutingAssembly().LoadFont("fonts", fontTitle);
            return new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        private static Typeface TypefaceFor(char c, out GlyphTypeface gt, out ushort glyphIndex)
        {
            gt = null;
            glyphIndex = 42;
            if (c == 0)
                return null;
            foreach (var typeface in Typefaces)
                if (typeface.TryGetGlyphTypeface(out gt) && gt.CharacterToGlyphMap.TryGetValue(c, out glyphIndex))
                    return typeface;
            return null;
        }

        private static double PixelsToPoints(double size)
        {
            // pixels to points, cf.: http://stackoverflow.com/a/139712/2592915
            return size * (72.0 / Dpi);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static int GetDpi()
        {
            // How can I get the DPI in WPF?, cf.: http://stackoverflow.com/a/12487917/2592915
            var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            return (int)dpiProperty.GetValue(null, null);
        }
    }
}
