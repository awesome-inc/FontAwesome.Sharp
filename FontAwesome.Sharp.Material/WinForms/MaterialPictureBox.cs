using System.ComponentModel;

namespace FontAwesome.Sharp.Material;

[ToolboxItem(true)]
[DesignTimeVisible(true)]
[Description("A windows forms picture box supporting material design icons")]
public class MaterialPictureBox : IconPictureBox<MaterialIcons>
{
    public MaterialPictureBox() : base(MaterialDesignFont.WinForms.Value)
    {
    }
}
