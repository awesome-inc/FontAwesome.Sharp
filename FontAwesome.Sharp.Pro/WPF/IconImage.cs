using System;
using System.Windows.Media;

namespace FontAwesome.Sharp.Pro
{
    public class IconImage : IconImageBase<ProIcons>
    {
        protected override ImageSource ImageSourceFor(ProIcons icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return ProFonts.For(icon)
                .ToImageSource(icon, Foreground, size);
        }
    }
}
