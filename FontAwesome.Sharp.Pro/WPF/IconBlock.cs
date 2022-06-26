using System.Windows.Media;

namespace FontAwesome.Sharp.Pro;

public class IconBlock : IconBlockBase<ProIcons>
{
    public IconBlock() : base(ProFonts.WpfFontFor(ProIcons.Abacus))
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return ProFonts.WpfFontFor(icon);
    }
}
