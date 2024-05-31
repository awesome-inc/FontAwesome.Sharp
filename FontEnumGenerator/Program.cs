using System;
using System.IO;
using System.Text;
using CommandLine;

namespace FontEnumGenerator;

internal static class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(GenerateFontEnum);
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    private sealed class Options
    {
        [Option(Default = @"Content\all.css", HelpText = "Input font css file to be processed.")]
        public string Css { get; set; }

        [Option(Default = "\\.fa.(.+):before", HelpText = "RegEx Pattern to match a font icon class.")]
        public string Pattern { get; set; }

        [Option(Default = false, HelpText = "Have distinct enum codes (skip duplicates).")]
        public bool Distinct { get; set; } = false;

        [Option(Default = "IconChar", HelpText = "Class & Output file to be generated.")]
        public string Name { get; set; }

        [Option(Default = "FontAwesome.Sharp", HelpText = "Output namespace.")]
        public string NameSpace { get; set; }

    }

    private static void GenerateFontEnum(Options opts)
    {
        var fontParser = new FontParser {CssFile = opts.Css, Pattern = opts.Pattern, Distinct = opts.Distinct};
        var items = fontParser.Parse();
        Console.WriteLine($"Matched {items.Count} icons from '{fontParser.CssFile}' using '{fontParser.Pattern}'");

        var builder = new StringBuilder();
        builder.AppendLine(string.Format(Header, opts.Name, opts.NameSpace));
        items.ForEach(item => builder.AppendLine($"    {item.Class} = {item.Code},"));
        builder.AppendLine(Footer);

        var path = $"{opts.Name}.cs";
        File.WriteAllText(path, builder.ToString());
        Console.WriteLine($"Generated '{path}'.");
    }

    private static readonly string Header = "namespace {1};" + Environment.NewLine +
                                            Environment.NewLine +
                                            "// ReSharper disable All" + Environment.NewLine +
                                            "public enum {0} : int" + Environment.NewLine +
                                            "{{" + Environment.NewLine +
                                            "    None = 0,";
    private static readonly string Footer = "}";
}
