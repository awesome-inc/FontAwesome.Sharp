using System;
using System.Reflection;

namespace FontAwesome.Sharp.Material;

using WinFormsFont = System.Drawing.FontFamily;
using WpfFont = System.Windows.Media.FontFamily;
internal static class MaterialDesignFont
{
    public const string FontName = "Material Design Icons";
    public static readonly Lazy<WinFormsFont> WinForms = new(LoadWinFormsFont);
    public static readonly Lazy<WpfFont> Wpf = new(LoadWpfFont);

    private static readonly Assembly FontAssembly = typeof(MaterialDesignFont).Assembly;
    private static WpfFont LoadWpfFont()
    {
        return FontAssembly.LoadFont("fonts", FontName);
    }

    private static WinFormsFont LoadWinFormsFont()
    {
        return FontAssembly.LoadResourceFont("fonts", "materialdesignicons-webfont.ttf");
    }
}
