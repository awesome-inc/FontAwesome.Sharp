using FontAwesome.Sharp;

namespace TestForms.MaterialDesign
{
    public class MaterialToolStripButton : IconToolStripButton<MaterialIcons>
    {
        public MaterialToolStripButton() : base(MaterialDesignFont.FontFamily)
        {
        }
    }
}
