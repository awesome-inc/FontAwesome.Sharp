using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public abstract class IconBase<TIconBlock, TIcon> : MarkupExtension
        where TIconBlock : IconBlockBase<TIcon>, new()
        where TIcon : struct, IConvertible, IComparable, IFormattable
    {
        private readonly TIconBlock _iconBlock;

        protected IconBase(TIcon icon)
        {
            _iconBlock = new TIconBlock
            {
                Icon = icon
            };
        }

        public Brush Foreground
        {
            get => _iconBlock.Foreground;
            set => _iconBlock.Foreground = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _iconBlock;
        }
    }
}