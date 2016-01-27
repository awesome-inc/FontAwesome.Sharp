using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FontAwesome.Sharp
{
    class Dummy
    {
        class Fa
        {
            public string Class { get; set; }
            public string Code { get; set; }
        }

        void a()
        {
            var css = "...";
            // http://derekslager.com/blog/posts/2007/09/a-better-dotnet-regular-expression-tester.ashx
            // NOTE: avoid double quotes on the website
            var regEx = new Regex(@"\.fa-(.+):before\s*\{\s*content:\s*""\\(.+)"";\s*}", RegexOptions.Multiline);

            var fas = regEx.Matches(css).OfType<Match>()
                .Select(match => new Fa {Class = ValidIdentifier(match.Groups[1].Value), Code = match.Groups[2].Value})
                .OrderBy(x => x.Class)
                .ToList();

            fas.ForEach(fa => Trace.TraceInformation("\t\t public const string {0} = \"\\u{1}\";", fa.Class, fa.Code));
        }

        // c.f.: http://blog.visualt4.com/2009/02/creating-valid-c-identifiers.html
        private static readonly CodeDomProvider Csharp = CodeDomProvider.CreateProvider("C#");
        private static readonly Regex InvalidChars = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]", RegexOptions.Compiled);
        private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

        private static string ValidIdentifier(string value)
        {
            // cf.: http://stackoverflow.com/questions/1206019/converting-string-to-title-case-in-c-sharp
            // dash to camel case
            var name = TextInfo.ToTitleCase(value.Replace('-', ' '));

            // Compliant with item 2.4.2 of the C# specification
            var ret = InvalidChars.Replace(name, String.Empty);
            //The identifier must start with a character or a "_"
            if (!char.IsLetter(ret, 0) || !Csharp.IsValidIdentifier(ret))
                ret = string.Concat("_", ret);
            return ret;
        }

    }
}
