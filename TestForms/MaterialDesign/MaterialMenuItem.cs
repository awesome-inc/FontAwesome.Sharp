using FontAwesome.Sharp;
using TestWpf.MaterialDesign;

namespace TestForms.MaterialDesign
{
    public class MaterialMenuItem : IconMenuItem<MaterialIcons>
    {
        public MaterialMenuItem() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
