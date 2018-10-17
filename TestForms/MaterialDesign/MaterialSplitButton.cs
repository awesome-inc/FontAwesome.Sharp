using FontAwesome.Sharp;
using TestWpf.MaterialDesign;

namespace TestForms.MaterialDesign
{
    public class MaterialSplitButton : IconSplitButton<MaterialIcons>
    {
        public MaterialSplitButton() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
