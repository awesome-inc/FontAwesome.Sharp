using System;
using System.Reflection;

namespace FontAwesome.Sharp.Material
{
    using WinFormsFont = System.Drawing.FontFamily;
    using WpfFont = System.Windows.Media.FontFamily;
    internal static class MaterialDesignFont
    {
        public static readonly Lazy<WinFormsFont> WinForms = new Lazy<WinFormsFont>(LodWinFormsFont);
        public static readonly Lazy<WpfFont> Wpf = new Lazy<WpfFont>(LoadWpfFont);

        private static readonly Assembly FontAssembly = typeof(MaterialDesignFont).Assembly;
        private static WpfFont LoadWpfFont()
        {
            return FontAssembly.LoadFont("fonts", "Material Design Icons");
        }

        private static WinFormsFont LodWinFormsFont()
        {
            return FontAssembly.LoadResourceFont("fonts", "materialdesignicons-webfont.ttf");
        }
    }
}
