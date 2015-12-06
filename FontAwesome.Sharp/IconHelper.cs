using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    // cf.: 
    // * http://stackoverflow.com/questions/23108181/changing-font-icon-in-wpf-using-font-awesome
    // * http://www.codeproject.com/Tips/634540/Using-Font-Icons
    public static class IconHelper
    {
        public static readonly FontFamily FontAwesome = new FontFamily(new Uri("pack://application:,,,"),
            "/FontAwesome.Sharp;component/fonts/#FontAwesome");
        public static readonly Brush DefaultBrush = SystemColors.WindowTextBrush; // this is TextBlock default brush
        public const double DefaultSize = 16.0;

        public static ImageSource ToImageSource(this IconChar iconChar, 
            Brush foregroundBrush = null, double size = DefaultSize)
        {
            var text = char.ConvertFromUtf32((int) iconChar);
            return ToImageSource(text, foregroundBrush ?? DefaultBrush, size);
        }

        public static ImageSource ToImageSource(string text, 
            Brush foregroundBrush = null, double size = DefaultSize)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            var glyphIndexes = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            for (var n = 0; n < text.Length; n++)
            {
                ushort glyphIndex;
                try
                {
                    glyphIndex = GlyphTypeface.CharacterToGlyphMap[text[n]];
                }
                catch (Exception)
                {
                    glyphIndex = 42;
                }
                glyphIndexes[n] = glyphIndex;

                var width = GlyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
                advanceWidths[n] = width;
            }

            try
            {
                // pixels to points, cf.: http://stackoverflow.com/a/139712/2592915
                var fontSize = size * (72.0 / 96.0);
                var glyphRun = new GlyphRun(GlyphTypeface, 0, false, fontSize, glyphIndexes,
                    new Point(0, 0), advanceWidths, null, null, null, null, null, null);

                var glyphRunDrawing = new GlyphRunDrawing(foregroundBrush ?? DefaultBrush, glyphRun);
                return new DrawingImage(glyphRunDrawing);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error generating GlyphRun : {ex.Message}");
            }
            return null;
        }


        private static readonly GlyphTypeface GlyphTypeface;

        static IconHelper()
        {
            var typeface = new Typeface(FontAwesome, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            if (typeface.TryGetGlyphTypeface(out GlyphTypeface)) return;
            typeface = new Typeface(new FontFamily(new Uri("pack://application:,,,"), FontAwesome.Source),
                FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            if (!typeface.TryGetGlyphTypeface(out GlyphTypeface))
                throw new InvalidOperationException("No glyphtypeface found");
        }
    }
}
