using System;
using System.Windows.Markup;

namespace FontAwesome.Sharp
{
    public class ToTextBase<TEnum> : MarkupExtension where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        private readonly string _text;

        public ToTextBase(TEnum icon)
        {
            _text = new string(icon.ToChar(), 1);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _text;
        }
    }
}