using System.Drawing;
using System.Reflection;
using FontAwesome.Sharp;

namespace TestForms.MaterialDesign
{
    internal static class MaterialDesignFont
    {
        internal static readonly FontFamily FontFamily =
            Assembly.GetExecutingAssembly().LoadResourceFont("fonts", "materialdesignicons-webfont.ttf");
    }
}
