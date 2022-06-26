using System.ComponentModel;

namespace FontAwesome.Sharp.Material;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms menu button supporting material design icons")]
public class MaterialMenuItem : IconMenuItem<MaterialIcons>
{
    public MaterialMenuItem() : base(MaterialDesignFont.WinForms.Value)
    {
    }
}
