using System;
using System.IO;
using System.Text;
using CommandLine;

namespace FontEnumGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(GenerateFontEnum);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private class Options
        {
            [Option(Default = @"Content\fontawesome.css", HelpText = "Input font css file to be processed.")]
            public string Css { get; set; }

            [Option(Default = "\\.fa.(.+):before", HelpText = "RegEx Pattern to match a font icon class.")]
            public string Pattern { get; set; }


            [Option(Default = "IconChar", HelpText = "Class & Output file to be generated.")]
            public string Name { get; set; }

            [Option(Default = "FontAwesome.Sharp", HelpText = "Output namespace.")]
            public string NameSpace { get; set; }

        }

        private static void GenerateFontEnum(Options opts)
        {
            var fontParser = new FontParser {CssFile = opts.Css, Pattern = opts.Pattern};

            var items = fontParser.Parse();
            Console.WriteLine($"Matched {items.Count} icons from '{fontParser.CssFile}' using '{fontParser.Pattern}'");

            var builder = new StringBuilder();
            builder.AppendLine(string.Format(Header, opts.Name, opts.NameSpace));
            items.ForEach(item => builder.AppendLine($"        {item.Class} = 0x{item.Code},"));
            builder.AppendLine(Footer);

            var path = $"{opts.Name}.cs";
            File.WriteAllText(path, builder.ToString());
            Console.WriteLine($"Generated '{path}'.");
        }

        private static readonly string Header = "// ReSharper disable InconsistentNaming" + Environment.NewLine +
                                                "// ReSharper disable IdentifierTypo" + Environment.NewLine +
                                                "// ReSharper disable UnusedMember.Global" + Environment.NewLine +
                                                "// ReSharper disable once CheckNamespace" + Environment.NewLine +
                                                "namespace {1}" + Environment.NewLine +
                                                "{{" + Environment.NewLine +
                                                "    public enum {0}" + Environment.NewLine +
                                                "    {{" + Environment.NewLine +
                                                "        None = 0,";

        private static readonly string Footer = "    }" + Environment.NewLine +
                                                "}";
    }
}
