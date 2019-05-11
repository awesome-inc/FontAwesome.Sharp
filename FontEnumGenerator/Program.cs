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

            [Option(Default = ".fa-", HelpText = "Prefix to match a font icon class.")]
            public string Prefix { get; set; }

            [Option(Default = "IconChar", HelpText = "Class & Output file to be generated.")]
            public string Name { get; set; }
        }

        private static void GenerateFontEnum(Options opts)
        {
            var items = CssFont.Parse(opts.Css, opts.Prefix);
            var builder = new StringBuilder();
            builder.AppendLine(string.Format(Header, opts.Name));
            items.ForEach(item => builder.AppendLine($"        {item.Class} = 0x{item.Code},"));
            builder.AppendLine(Footer);
            File.WriteAllText($"{opts.Name}.cs", builder.ToString());
        }

        private static readonly string Header = "// ReSharper disable InconsistentNaming" + Environment.NewLine +
                                                "// ReSharper disable IdentifierTypo" + Environment.NewLine +
                                                "// ReSharper disable UnusedMember.Global" + Environment.NewLine +
                                                "namespace FontAwesome.Sharp" + Environment.NewLine +
                                                "{{" + Environment.NewLine +
                                                "    public enum {0}" + Environment.NewLine +
                                                "    {{" + Environment.NewLine +
                                                "        None = 0,";

        private static readonly string Footer = "    }" + Environment.NewLine +
                                                "}";
    }
}
