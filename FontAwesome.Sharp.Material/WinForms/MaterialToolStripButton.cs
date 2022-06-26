using System.ComponentModel;

namespace FontAwesome.Sharp.Material;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms tool strip button supporting material design icons")]
public class MaterialToolStripButton : IconToolStripButton<MaterialIcons>
{
    public MaterialToolStripButton() : base(MaterialDesignFont.WinForms.Value)
    {
    }
}
