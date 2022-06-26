using System.ComponentModel;
using System.Drawing;

namespace FontAwesome.Sharp.Pro;

/// <summary>
/// A windows forms button supporting font awesome pro icons
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms button supporting font awesome pro icons")]
public class IconButton : IconButton<ProIcons>
{
    public IconButton() : base(ProIcons.Abacus.WinFormsFontFor())
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return icon.WinFormsFontFor();
    }
}
