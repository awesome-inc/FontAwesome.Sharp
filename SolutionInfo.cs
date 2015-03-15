using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription(@"© Awesome Inc. 2015")]

[assembly: AssemblyCompany("Awesome Inc.")]
[assembly: AssemblyProduct("FontAwesome.WPF")]
[assembly: AssemblyCopyright("Copyright © Awesome Inc. 2015")]
[assembly: AssemblyTrademark("Awesome Inc.")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.MainAssembly)]
