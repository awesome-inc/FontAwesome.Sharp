using System.ComponentModel;
using System.Drawing;

namespace FontAwesome.Sharp.Pro;

/// <summary>
/// A windows forms menu item supporting font awesome pro icons
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms menu item supporting font awesome pro icons")]
public class IconMenuItem : IconMenuItem<ProIcons>
{
    public IconMenuItem() : base(ProIcons.Abacus.WinFormsFontFor())
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return icon.WinFormsFontFor();
    }
}
