using FontAwesome.Sharp;

namespace TestForms.MaterialDesign
{
    public class MaterialButton : IconButton<MaterialIcons>
    {
        public MaterialButton() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
