using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FontEnumGenerator
{
    internal class FontParser
    {
        public string CssFile { get; set; }
        public string Pattern { get; set; } = @"\.fa-(.+):before";

        private const string ContentPattern = @"\s*\{\s*content:\s*""\\(.+)"";\s*}";

        public List<FontEnumItem> Parse()
        {
            var css = File.ReadAllText(CssFile);

            var allPattern = Pattern + ContentPattern;
            var regEx = new Regex(allPattern, RegexOptions.Multiline);

            var items = regEx.Matches(css)
                .Select(match => new FontEnumItem { Class = ValidIdentifier(match.Groups[1].Value), Code = match.Groups[2].Value })
                .OrderBy(x => x.Class)
                .ToList();

            return items;
        }

        // c.f.: http://blog.visualt4.com/2009/02/creating-valid-c-identifiers.html
        private static readonly CodeDomProvider Csharp = CodeDomProvider.CreateProvider("C#");
        private static readonly Regex InvalidChars = new(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.Compiled);
        private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

        private static string ValidIdentifier(string value)
        {
            // cf.: http://stackoverflow.com/questions/1206019/converting-string-to-title-case-in-c-sharp
            // dash to camel case
            var name = TextInfo.ToTitleCase(value.Replace('-', ' '));

            // Compliant with item 2.4.2 of the C# specification
            var ret = InvalidChars.Replace(name, string.Empty);
            //The identifier must start with a character or a "_"
            if (!char.IsLetter(ret, 0) || !Csharp.IsValidIdentifier(ret))
                ret = string.Concat("_", ret);
            return ret;
        }
    }
}
