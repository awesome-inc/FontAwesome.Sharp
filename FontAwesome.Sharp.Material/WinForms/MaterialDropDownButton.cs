using System.ComponentModel;

namespace FontAwesome.Sharp.Material;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms drop down button supporting material design icons")]
public class MaterialDropDownButton : IconDropDownButton<MaterialIcons>
{
    public MaterialDropDownButton() : base(MaterialDesignFont.WinForms.Value)
    {
    }
}
