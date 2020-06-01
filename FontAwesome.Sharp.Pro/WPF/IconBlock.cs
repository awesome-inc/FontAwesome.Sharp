using System.Windows.Media;

namespace FontAwesome.Sharp.Pro
{
    public class IconBlock : IconBlockBase<ProIcons>
    {
        public IconBlock() : base(ProFonts.For(ProIcons.Abacus))
        {
        }

        protected override FontFamily FontFor(ProIcons icon)
        {
            return ProFonts.For(icon);
        }
    }
}
