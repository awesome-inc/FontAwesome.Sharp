using FontAwesome.Sharp;
using TestWpf.MaterialDesign;

namespace TestForms.MaterialDesign
{
    public class MaterialDropDownButton : IconDropDownButton<MaterialIcons>
    {
        public MaterialDropDownButton() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
