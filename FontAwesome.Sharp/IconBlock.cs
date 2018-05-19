using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FontAwesome.Sharp
{
    // adapted from https://bitbucket.org/ioachim/fontawesome.wpf
    public class IconBlock : TextBlock
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(IconChar), typeof(IconBlock),
            new PropertyMetadata(IconChar.None, OnIconPropertyChanged));

        public IconBlock()
        {
            VerticalAlignment = VerticalAlignment.Center;
            TextAlignment = TextAlignment.Center;

            var descriptor = DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(IconBlock));
            descriptor.AddValueChanged(this, OnTextValueChanged);
            var fontFamily = IconHelper.FontFor(Icon);
            if (fontFamily != null)
                FontFamily = fontFamily;
        }

        public IconChar Icon
        {
            get => (IconChar) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var iconBlock = d as IconBlock;
            if (iconBlock == null) return;
#if NET40
            iconBlock.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
#endif
            iconBlock.SetValue(FontFamilyProperty, IconHelper.FontFor(iconBlock.Icon));
            iconBlock.SetValue(TextAlignmentProperty, TextAlignment.Center);
            iconBlock.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            iconBlock.SetValue(TextProperty, char.ConvertFromUtf32((int) e.NewValue));
        }

        private void OnTextValueChanged(object sender, EventArgs e)
        {
            var str = Text;
            if (str.Length != 1 || !Enum.IsDefined(typeof(IconChar), char.ConvertToUtf32(str, 0)))
                throw new FormatException();
        }
    }
}
