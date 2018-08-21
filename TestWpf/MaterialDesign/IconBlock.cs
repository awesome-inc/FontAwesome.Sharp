using System.Reflection;
using System.Windows.Media;
using FontAwesome.Sharp;

namespace TestWpf.MaterialDesign
{
    public class IconBlock : IconBlockBase<MaterialIcons>
    {
        private static readonly FontFamily MaterialDesign = Assembly.GetExecutingAssembly().GetFont("fonts", "Material Design Icons");

        public IconBlock() : base(MaterialDesign)
        {
        }
    }
}
