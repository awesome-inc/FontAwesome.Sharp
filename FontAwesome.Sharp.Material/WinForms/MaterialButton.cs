using System.ComponentModel;

namespace FontAwesome.Sharp.Material;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms button supporting material design icons")]
public class MaterialButton : IconButton<MaterialIcons>
{
    public MaterialButton() : base(MaterialDesignFont.WinForms.Value)
    {
    }
}
