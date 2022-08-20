using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;

namespace FontAwesome.Sharp.Pro;

using WinFormsFont = System.Drawing.FontFamily;
using WpfFont = System.Windows.Media.FontFamily;
using WpfTypeface = System.Windows.Media.Typeface;

internal static class ProFonts
{
    [field: SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static bool ThrowOnNullFonts { get; } = false;
    internal static WpfTypeface Throw(string message)
    {
        if (ThrowOnNullFonts) throw new InvalidOperationException(message);
        return default;
    }


    #region WinForms
    internal static readonly Dictionary<int, string> FontTitles = new()
    {
        { (int)ProIconFont.Regular,  "Font Awesome 6 Pro Regular"}, // fa-regular-400-pro.ttf
        { (int)ProIconFont.Solid, "Font Awesome 6 Pro Solid"}, // fa-solid-900-pro.ttf
        { (int)ProIconFont.Light, "Font Awesome 6 Pro Light"}, // fa-light-300-pro.ttf
        { (int)ProIconFont.Thin, "Font Awesome 6 Pro Thin"}, // fa-thin-100-pro.ttf
        //"Font Awesome 6 Duotone Solid", // fa-duotone-900-pro.ttf
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
        _wpfTypefaces = assembly.LoadTypefaces(path, FontTitles.Values);
        _wpfInitialized = true;
    }

    public static WpfFont WpfFontFor(this ProIcons icon, ProIconFont iconFont = ProIconFont.Auto)
    {
        var typeFace = TypeFaceFor(icon, iconFont);
        return typeFace?.FontFamily;
    }

    private static readonly Dictionary<int, WpfTypeface> TypefaceForStyle = new();

    internal static WpfTypeface TypeFaceFor(this ProIcons icon, ProIconFont iconFont)
    {
        if (iconFont == ProIconFont.Auto) return WpfTypefaces.Find(icon, out _, out _);
        var key = (int)iconFont;
        if (TypefaceForStyle.TryGetValue(key, out var typeFace)) return typeFace;
        if (!FontTitles.TryGetValue(key, out var name))
            return Throw($"No font loaded for style: {iconFont}");

        typeFace = WpfTypefaces.FirstOrDefault(t => t.FontFamily.Source.EndsWith(name));
        if (typeFace == null)
            return Throw($"No font loaded for '{name}'");

        TypefaceForStyle.Add(key, typeFace);
        return typeFace;
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
