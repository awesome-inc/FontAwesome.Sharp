using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public class IconImage : Image
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(IconChar), typeof(IconImage),
            new PropertyMetadata(IconChar.None, OnIconPropertyChanged));
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(IconImage),
            new PropertyMetadata(IconHelper.DefaultBrush, OnForegroundPropertyChanged));

        public IconChar Icon
        {
            get { return (IconChar)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var iconChar = (IconChar)e.NewValue;
            var brush = (Brush)d.GetValue(ForegroundProperty);
            var imageSource = iconChar.ToImageSource(brush);
            d.SetValue(SourceProperty, imageSource);
        }

        private static void OnForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var iconChar = (IconChar)d.GetValue(IconProperty);
            var brush = (Brush)d.GetValue(ForegroundProperty);
            var imageSource = iconChar.ToImageSource(brush);
            d.SetValue(SourceProperty, imageSource);
        }
    }
}
