using System;
using System.Windows.Media;
using FontAwesome.Sharp;

namespace TestWpf.MaterialDesign
{
    public class IconImage : IconImageBase<MaterialIcons>
    {
        protected override ImageSource ImageSourceFor(MaterialIcons icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return MaterialDesignFont.FontFamily.ToImageSource(icon, Foreground, size);
        }
    }
}
