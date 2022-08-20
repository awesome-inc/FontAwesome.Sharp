using System.Windows.Media;

namespace FontAwesome.Sharp.Pro;

public class IconBlock : IconBlockBase<ProIcons>
{
    public IconBlock() : base(ProIcons.Abacus.WpfFontFor())
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return icon.WpfFontFor();
    }
}
