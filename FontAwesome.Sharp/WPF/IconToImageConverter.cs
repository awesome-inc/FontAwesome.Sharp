using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    [ValueConversion(typeof(IconChar), typeof(Image))]
    public class IconToImageConverter : IValueConverter, IHaveIconFont
    {
        public Brush Foreground { get; set; }
        public Style ImageStyle { get; set; }
        public IconFont IconFont { get; set; } = IconFont.Auto;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var icon = (IconChar) value;
            var image = new IconImage
            {
                Icon = icon, IconFont = IconFont
            };

            if (Foreground != null)
                image.Foreground = Foreground;

            if (ImageStyle != null)
                image.Style = ImageStyle;

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
