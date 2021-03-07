using System;
using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public class IconImage : IconImageBase<IconChar>, IHaveIconFont
    {
        protected override ImageSource ImageSourceFor(IconChar icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return icon.ToImageSource(IconFont, Foreground, size);
        }

        public static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont),
            typeof(IconFont), typeof(IconImage),
            new PropertyMetadata(default(IconFont), OnIconFontPropertyChanged));

        private static void OnIconFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is IconImage iconImage)) return;
            var imageSource = iconImage.ImageSourceFor(iconImage.Icon);
            iconImage.SetValue(SourceProperty, imageSource);
        }


        public IconFont IconFont {
            get => (IconFont)GetValue(IconFontProperty);
            set => SetValue(IconFontProperty, value);
        }

    }
}
