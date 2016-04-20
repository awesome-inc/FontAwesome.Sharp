using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FontAwesome.Sharp
{
    // adapted from https://bitbucket.org/ioachim/fontawesome.wpf
    public class IconBlock : TextBlock
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(IconChar), typeof(IconBlock), 
            new PropertyMetadata(IconChar.None, OnIconPropertyChanged));

        public IconChar Icon
        {
            get { return (IconChar)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
#if NET40
            d.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
#endif
            d.SetValue(FontFamilyProperty, IconHelper.FontAwesome);
            d.SetValue(TextAlignmentProperty, TextAlignment.Center);
            d.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);

            d.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }

        public IconBlock()
        {
            FontFamily = IconHelper.FontAwesome;
            VerticalAlignment = VerticalAlignment.Center;
            TextAlignment = TextAlignment.Center;

            var descriptor = DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(IconBlock));
            descriptor.AddValueChanged(this, OnTextValueChanged);
            FontFamily = IconHelper.FontAwesome;
        }

        private void OnTextValueChanged(object sender, EventArgs e)
        {
            var str = Text;
            if (str.Length != 1 || !Enum.IsDefined(typeof(IconChar), char.ConvertToUtf32(str, 0)))
                throw new FormatException();
        }
    }
}
