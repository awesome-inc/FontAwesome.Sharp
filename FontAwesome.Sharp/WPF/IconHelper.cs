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
        #region Public

        public static bool ThrowOnNullFonts = false;

        public static readonly IconChar[] Orphans = {
            IconChar.None
            // not contained in any of the ttf-fonts!
            ,IconChar.FontAwesomeLogoFull
        };

        /// <summary>
        /// All valid icons.
        /// </summary>
        public static readonly IconChar[] Icons = Enum.GetValues(typeof(IconChar))
            .Cast<IconChar>().Except(Orphans).ToArray();

        /// <summary>
        /// Default brush / color.
        /// </summary>
        public static readonly Brush DefaultBrush = SystemColors.WindowTextBrush; // this is TextBlock default brush

        /// <summary>
        /// Default icon size in pixels.
        /// </summary>
        public const int DefaultSize = 48;

        /// <summary>
        /// Load the specified font from assembly resource stream
        /// </summary>
        /// <param name="assembly">The assembly to load from</param>
        /// <param name="path">The resource path in the assembly</param>
        /// <param name="fontTitle">The resource name</param>
        /// <returns></returns>
        public static FontFamily LoadFont(this Assembly assembly, string path, string fontTitle)
        {
            return new(BaseUri, $"./{assembly.GetName().Name};component/{path}/#{fontTitle}");
        }

        /// <summary>
        /// Renders an image for the specified font and icon
        /// </summary>
        /// <typeparam name="TEnum">The icon enum type</typeparam>
        /// <param name="fontFamily">The icon font</param>
        /// <param name="icon">The icon to render</param>
        /// <param name="brush">The icon brush / color</param>
        /// <param name="size">The icon size in pixels</param>
        /// <returns>The rendered image</returns>
        public static ImageSource ToImageSource<TEnum>(this FontFamily fontFamily, TEnum icon,
            Brush brush = null, double size = DefaultSize)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return fontFamily.GetTypefaces().Find(icon, out var gt, out var glyphIndex) != null
                ? ToImageSource(brush, size, gt, glyphIndex)
                : null;
        }

        /// <summary>
        /// Renders an image for the specified font and icon
        /// </summary>
        /// <param name="iconChar">The icon to render</param>
        /// <param name="brush">The icon brush / color</param>
        /// <param name="size">The icon size in pixels</param>
        /// <returns>The rendered image</returns>
        public static ImageSource ToImageSource(this IconChar iconChar,
            Brush brush = null, double size = DefaultSize)
        {
            var typeFace = Typefaces.Find(iconChar, out var gt, out var glyphIndex);
            return typeFace == null ? null : ToImageSource(brush, size, gt, glyphIndex);
        }

        /// <summary>
        /// Renders an image for the specified font, style and icon
        /// </summary>
        /// <param name="iconChar">The icon to render</param>
        /// <param name="iconFont">The icon font style</param>
        /// <param name="brush">The icon brush / color</param>
        /// <param name="size">The icon size in pixels</param>
        /// <returns>The rendered image</returns>
        public static ImageSource ToImageSource(this IconChar iconChar, IconFont iconFont,
            Brush brush = null, double size = DefaultSize)
        {
            var typeface = TypefaceFor(iconChar, iconFont);
            if (typeface == null) return null;
            return typeface.TryFind(iconChar.UniCode(), out var gt, out var glyphIndex) ? ToImageSource(brush, size, gt, glyphIndex) : null;
        }

        /// <summary>
        /// Convert icon code to UTF-32 unicode character
        /// </summary>
        /// <typeparam name="TEnum">The icon enum type</typeparam>
        /// <param name="icon">The icon</param>
        /// <returns>Character code</returns>
        public static string ToChar<TEnum>(this TEnum icon) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return char.ConvertFromUtf32(icon.UniCode());
        }

        /// <summary>
        /// Load typefaces from assembly resources
        /// </summary>
        /// <param name="assembly">The assembly to load from</param>
        /// <param name="path">The resource path (directory)</param>
        /// <param name="fontTitles">The font resource item names</param>
        /// <returns>The loaded typefaces</returns>
        public static Typeface[] LoadTypefaces(this Assembly assembly, string path,
            IEnumerable<string> fontTitles)
        {
            return fontTitles.Select(fontTitle =>
            {
                var fontFamily = assembly.LoadFont(path, fontTitle);
                return new Typeface(fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            }).ToArray();
        }

        /// <summary>
        /// Find the typeface containing the specified icon
        /// </summary>
        /// <typeparam name="TEnum">The icon enum type</typeparam>
        /// <param name="typefaces"></param>
        /// <param name="icon">The icon</param>
        /// <param name="gt">The found font face</param>
        /// <param name="glyphIndex">The glyph index into the font face for the specified icon</param>
        /// <returns></returns>
        public static Typeface Find<TEnum>(this IEnumerable<Typeface> typefaces,
            TEnum icon, out GlyphTypeface gt, out ushort glyphIndex)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var iconCode = icon.UniCode();
            gt = null;
            glyphIndex = NoSuchGlyph;
            foreach (var typeface in typefaces)
            {
                if (typeface.TryFind(iconCode, out gt, out glyphIndex))
                    return typeface;
            }
            return null;
        }

        private const ushort NoSuchGlyph = 42;
        private static bool TryFind(this Typeface typeface, int iconCode, out GlyphTypeface gt, out ushort glyphIndex)
        {
            gt = null;
            glyphIndex = NoSuchGlyph;
            return typeface.TryGetGlyphTypeface(out gt) && gt.CharacterToGlyphMap.TryGetValue(iconCode, out glyphIndex);
        }

        #endregion

        #region Internal
        internal static FontFamily FontFor(IconChar iconChar)
        {
            return TypefaceFor(iconChar)?.FontFamily;
        }

        internal static FontFamily FontFor(IconChar iconChar, IconFont iconFont)
        {
            return TypefaceFor(iconChar, iconFont)?.FontFamily;
        }

        internal static Typeface TypefaceFor(IconChar iconChar)
        {
            return Orphans.Contains(iconChar) ? null : Typefaces.Find(iconChar.UniCode(), out _, out _);
        }


        internal static Typeface TypefaceFor(IconChar iconChar, IconFont iconFont)
        {
            if (iconFont == IconFont.Auto) return TypefaceFor(iconChar);
            var key = (int)iconFont;
            if (TypefaceForStyle.TryGetValue(key, out var typeFace)) return typeFace;
            if (!FontTitles.TryGetValue(key, out var name))
                return Throw($"No font loaded for style: {iconFont}");

            typeFace = Typefaces.FirstOrDefault(t => t.FontFamily.Source.EndsWith(name));
            if (typeFace == null)
                return Throw($"No font loaded for '{name}'");

            TypefaceForStyle.Add(key, typeFace);
            return typeFace;

        }
        private static readonly Dictionary<int, Typeface> TypefaceForStyle = new();

        internal static Typeface Throw(string message)
        {
            if (ThrowOnNullFonts) throw new InvalidOperationException(message);
            return default;
        }

        internal static readonly Uri BaseUri = new($"{System.IO.Packaging.PackUriHelper.UriSchemePack}://application:,,,/");

        #endregion

        #region Private
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

        private static int UniCode<TEnum>(this TEnum icon)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return icon.ToInt32(CultureInfo.InvariantCulture);
        }

        internal static readonly Dictionary<int, string> FontTitles = new Dictionary<int, string>()
        {
            { (int)IconFont.Regular,  "Font Awesome 5 Free Regular"}, // fa-regular-400.ttf
            { (int)IconFont.Solid, "Font Awesome 5 Free Solid"}, // fa-solid-900.ttf
            { (int)IconFont.Brands, "Font Awesome 5 Brands Regular"} // fa-brands-400.ttf
        };
        //internal static Dictionary<int, String> FontForStyle

        private static readonly Typeface[] Typefaces = typeof(IconHelper).Assembly.LoadTypefaces("fonts", FontTitles.Values);
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
        #endregion
    }
}
