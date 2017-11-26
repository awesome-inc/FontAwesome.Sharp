using System;
using System.Windows.Markup;

namespace FontAwesome.Sharp
{
    public class ToText : MarkupExtension
    {
        private readonly string _text;

        public ToText(IconChar iconChar)
        {
            _text = new string((char) iconChar, 1);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _text;
        }
    }
}