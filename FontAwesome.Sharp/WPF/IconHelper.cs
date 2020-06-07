using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
            if (fontFamily.GetTypefaces().Find(icon, out var gt, out var glyphIndex) == null)
                return null;
            return ToImageSource(foregroundBrush, size, gt, glyphIndex);
        }

        public static string ToChar<TEnum>(this TEnum icon) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return char.ConvertFromUtf32(icon.UniCode());
        }

        private static int UniCode<TEnum>(this TEnum icon)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return icon.ToInt32(CultureInfo.InvariantCulture);
        }

        public static ImageSource ToImageSource(this IconChar iconChar,
            Brush foregroundBrush = null, double size = DefaultSize)
        {
            var typeFace = Typefaces.Find(iconChar, out var gt, out var glyphIndex);
            return typeFace == null ? null : ToImageSource(foregroundBrush, size, gt, glyphIndex);
        }


        public static Typeface[] LoadTypefaces(this Assembly assembly, string path,
            params string[] fontTitles)
        {
            return fontTitles.Select(fontTitle =>
            {
                var fontFamily = assembly.LoadFont(path, fontTitle);
                return new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            }).ToArray();
        }

        public static Typeface Find<TEnum>(this IEnumerable<Typeface> typefaces,
            TEnum icon, out GlyphTypeface gt, out ushort glyphIndex)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            gt = null;
            glyphIndex = 42;
            var iconCode = icon.UniCode();
            foreach (var typeface in typefaces)
                if (typeface.TryGetGlyphTypeface(out gt) &&
                    gt.CharacterToGlyphMap.TryGetValue(iconCode, out glyphIndex))
                    return typeface;
            return null;
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

        internal static FontFamily FontFor(IconChar iconChar)
        {
            if (Orphans.Contains(iconChar)) return null;
            var typeFace = Typefaces.Find(iconChar.UniCode(), out _, out _);
            return typeFace?.FontFamily;
        }

        internal static readonly Uri BaseUri = new Uri($"{System.IO.Packaging.PackUriHelper.UriSchemePack}://application:,,,/");

        private static readonly string[] FontTitles =
        {
            "Font Awesome 5 Free Regular", // fa-regular-400.ttf
            "Font Awesome 5 Free Solid", // fa-solid-900.ttf
            "Font Awesome 5 Brands Regular" // fa-brands-400.ttf
        };

        private static readonly Typeface[] Typefaces = typeof(IconHelper).Assembly.LoadTypefaces("fonts", FontTitles);
        private static readonly int Dpi = GetDpi();

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
