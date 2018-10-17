using FontAwesome.Sharp;
using TestWpf.MaterialDesign;

namespace TestForms.MaterialDesign
{
    public class MaterialPictureBox : IconPictureBox<MaterialIcons>
    {
        public MaterialPictureBox() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
