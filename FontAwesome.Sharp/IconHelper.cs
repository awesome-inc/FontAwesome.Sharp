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

        public static ImageSource ToImageSource(this IconChar iconChar, Brush foregroundBrush = null)
        {
            var text = char.ConvertFromUtf32((int) iconChar);
            return ToImageSource(text, foregroundBrush ?? DefaultBrush,
                FontAwesome, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        private static ImageSource ToImageSource(string text, Brush foregroundBrush, 
            FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)
        {
            if (fontFamily == null || string.IsNullOrEmpty(text)) return null;
            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                typeface = new Typeface(new FontFamily(new Uri("pack://application:,,,"), fontFamily.Source), fontStyle, fontWeight, fontStretch);
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                    throw new InvalidOperationException("No glyphtypeface found");
            }

            var glyphIndexes = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            for (var n = 0; n < text.Length; n++)
            {
                ushort glyphIndex;
                try
                {
                    glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
                }
                catch (Exception)
                {
                    glyphIndex = 42;
                }
                glyphIndexes[n] = glyphIndex;

                var width = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
                advanceWidths[n] = width;
            }

            try
            {
                var gr = new GlyphRun(glyphTypeface, 0, false, 1.0, glyphIndexes,
                    new Point(0, 0), advanceWidths, null, null, null, null, null, null);

                var glyphRunDrawing = new GlyphRunDrawing(foregroundBrush, gr);
                return new DrawingImage(glyphRunDrawing);
            }
            catch (Exception ex)
            {
                // ReSharper disable one LocalizableElement
                Trace.TraceError("Error in generating Glyphrun : " + ex.Message);
            }
            return null;
        }
    }
}
