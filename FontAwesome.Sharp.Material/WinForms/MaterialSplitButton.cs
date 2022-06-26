using System.ComponentModel;

namespace FontAwesome.Sharp.Material;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms split button supporting material design icons")]
public class MaterialSplitButton : IconSplitButton<MaterialIcons>
{
    public MaterialSplitButton() : base(MaterialDesignFont.WinForms.Value)
    {
    }
}
