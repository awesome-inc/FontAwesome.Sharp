using System.Windows.Media;

namespace FontAwesome.Sharp
{
    // adapted from https://bitbucket.org/ioachim/fontawesome.wpf
    public class IconBlock: IconBlockBase<IconChar>
    {
        public IconBlock() : base(IconHelper.FontFor(IconChar.None))
        {}

        protected override FontFamily FontFor(IconChar icon)
        {
            return IconHelper.FontFor(icon);
        }
    }
}
