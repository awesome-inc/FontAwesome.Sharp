using System.ComponentModel;
using System.Drawing;

namespace FontAwesome.Sharp.Pro;

/// <summary>
/// A windows forms picture box supporting font awesome pro icons
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms picture box supporting font awesome pro icons")]
public class IconPictureBox : IconPictureBox<ProIcons>
{
    public IconPictureBox() : base(ProIcons.Abacus.WinFormsFontFor())
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return icon.WinFormsFontFor();
    }
}
