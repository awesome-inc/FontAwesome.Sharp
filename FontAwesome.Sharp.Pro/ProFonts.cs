using System;
using System.Reflection;
using System.Windows.Media;

namespace FontAwesome.Sharp.Pro
{
    internal static class ProFonts
    {
        private static bool _isInitialized;

        private static readonly string[] FontTitles =
        {
            "Font Awesome 5 Pro Regular", // fa-regular-400-pro.ttf
            "Font Awesome 5 Pro Solid", // fa-solid-900-pro.ttf
            "Font Awesome 5 Pro Light", // fa-light-300-pro.ttf
            //"Font Awesome 5 Duotone Solid", // fa-duotone-900-pro.ttf
        };

        private static Typeface[] _typefaces;

        public static Typeface[] Typefaces
        {
            get
            {
                if (!_isInitialized) Initialize();
                return _typefaces;
            }
        }

        public static void Initialize(Assembly fontsAssembly = null, string path = "fonts")
        {
            if (_isInitialized) throw new InvalidOperationException("Already initialized");
            var assembly = fontsAssembly ?? Assembly.GetEntryAssembly();
            _typefaces = assembly.LoadTypefaces(path, FontTitles);
            _isInitialized = true;
        }

        internal static FontFamily For(ProIcons icon)
        {
            var typeFace = Typefaces.Find(icon, out _, out _);
            return typeFace?.FontFamily;
        }
    }
}
