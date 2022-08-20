using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;

namespace FontAwesome.Sharp.Pro;

using WinFormsFont = System.Drawing.FontFamily;
using WpfFont = System.Windows.Media.FontFamily;
using WpfTypeface = System.Windows.Media.Typeface;

internal static class ProFonts
{
    #region WinForms
    private static readonly string[] FontTitles =
    {
        "Font Awesome 6 Pro Regular", // fa-regular-400-pro.ttf
        "Font Awesome 6 Pro Solid", // fa-solid-900-pro.ttf
        "Font Awesome 6 Pro Light", // fa-light-300-pro.ttf
        "Font Awesome 6 Pro Thin", // fa-thin-100-pro.ttf
        "Font Awesome 6 Duotone Solid", // fa-duotone-900-pro.ttf
    };

    private static bool _wpfInitialized;
    private static WpfTypeface[] _wpfTypefaces;

    public static IEnumerable<WpfTypeface> WpfTypefaces
    {
        get
        {
            if (!_wpfInitialized) InitializeWpf();
            return _wpfTypefaces;
        }
    }

    public static void InitializeWpf(Assembly fontsAssembly = null, string path = "fonts")
    {
        if (_wpfInitialized) throw new InvalidOperationException("Already initialized");
        var assembly = fontsAssembly ?? Assembly.GetEntryAssembly();
        _wpfTypefaces = assembly.LoadTypefaces(path, FontTitles);
        _wpfInitialized = true;
    }

    public static WpfFont WpfFontFor(this ProIcons icon)
    {
        var typeFace = WpfTypefaces.Find(icon, out _, out _);
        return typeFace?.FontFamily;
    }
    #endregion

    #region WinForms
    private static readonly string[] FontFiles =
    {
        "fa-regular-400-pro.ttf",
        "fa-solid-900-pro.ttf",
        "fa-light-300-pro.ttf",
        "fa-thin-100-pro.ttf",
        "fa-duotone-900-pro.ttf"
    };
    private static bool _winFormsInitialized;
    private static PrivateFontCollection _winFormsFonts;
    private static WinFormsFont _winFormsFallbackFont;

    public static PrivateFontCollection WinFormsFonts
    {
        get
        {
            if (!_winFormsInitialized) InitializeWinForms();
            return _winFormsFonts;
        }
    }

    public static void InitializeWinForms(Assembly fontsAssembly = null, string path = "fonts")
    {
        if (_winFormsInitialized) throw new InvalidOperationException("Already initialized");
        var assembly = fontsAssembly ?? Assembly.GetEntryAssembly();
        _winFormsFonts = new PrivateFontCollection();
        foreach (var fontFile in FontFiles.Reverse())
        {
            try
            {
                _winFormsFonts.AddFont(fontFile, assembly, path);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Could not load FontAwesome: {ex}");
                throw;
            }
        }

        _winFormsFallbackFont = _winFormsFonts.Families[0];
        _winFormsInitialized = true;
    }


    public static WinFormsFont WinFormsFontFor(this ProIcons icon)
    {
        var name = WpfFontFor(icon)?.Source;
        if (name == null) return _winFormsFallbackFont;
        return WinFormsFonts.Families.FirstOrDefault(f => name.EndsWith(f.Name, StringComparison.InvariantCultureIgnoreCase)) ?? _winFormsFallbackFont;
    }
    #endregion
}
