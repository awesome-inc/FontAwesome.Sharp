using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FontAwesome.WPF
{
    // adapted from https://bitbucket.org/ioachim/fontawesome.wpf
    public class IconBlock : TextBlock 
    {
        public IconChar Icon 
        {
            get { return (IconChar)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(IconChar), typeof(IconBlock), 
            new PropertyMetadata(IconChar.None, OnIconPropertyChanged));

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            d.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }

        public IconBlock() 
        {
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