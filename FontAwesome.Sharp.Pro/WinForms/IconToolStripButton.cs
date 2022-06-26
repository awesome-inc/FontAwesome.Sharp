using System.ComponentModel;
using System.Drawing;

namespace FontAwesome.Sharp.Pro;

/// <summary>
/// A windows forms tool strip button supporting font awesome pro icons
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms tool strip button supporting font awesome pro icons")]
public class IconToolStripButton : IconToolStripButton<ProIcons>
{
    public IconToolStripButton() : base(ProIcons.Abacus.WinFormsFontFor())
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return icon.WinFormsFontFor();
    }
}
