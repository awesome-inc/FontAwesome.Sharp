using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FontAwesome.WPF
{
    [ValueConversion(typeof(IconChar), typeof(Image))]
    public class IconToImageConverter : IValueConverter
    {
        public Brush Foreground { get; set; }
        public Style ImageStyle { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var icon = (IconChar) value;
            var image = new IconImage {Icon = icon};

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