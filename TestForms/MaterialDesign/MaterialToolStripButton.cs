using FontAwesome.Sharp;
using TestWpf.MaterialDesign;

namespace TestForms.MaterialDesign
{
    public class MaterialToolStripButton : IconToolStripButton<MaterialIcons>
    {
        public MaterialToolStripButton() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
