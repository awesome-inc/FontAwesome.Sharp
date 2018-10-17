using System;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public class IconImage : IconImageBase<IconChar>
    {
        protected override ImageSource ImageSourceFor(IconChar icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return icon.ToImageSource(Foreground, size);
        }
    }
}
