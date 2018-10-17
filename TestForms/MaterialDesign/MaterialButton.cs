using FontAwesome.Sharp;
using TestWpf.MaterialDesign;

namespace TestForms.MaterialDesign
{
    public class MaterialButton : IconButton<MaterialIcons>
    {
        public MaterialButton() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
