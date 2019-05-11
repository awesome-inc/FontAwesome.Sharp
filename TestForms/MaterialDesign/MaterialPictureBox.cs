using FontAwesome.Sharp;

namespace TestForms.MaterialDesign
{
    public class MaterialPictureBox : IconPictureBox<MaterialIcons>
    {
        public MaterialPictureBox() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
