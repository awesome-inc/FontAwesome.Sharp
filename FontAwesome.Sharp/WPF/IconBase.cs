using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public abstract class IconBase<TIconBlock, TIcon> : MarkupExtension
        where TIconBlock : IconBlockBase<TIcon>, new()
        where TIcon : struct, IConvertible, IComparable, IFormattable
    {
        protected readonly TIconBlock IconBlock;

        protected IconBase(TIcon icon)
        {
            IconBlock = new TIconBlock
            {
                Icon = icon
            };
        }

        public Brush Foreground
        {
            get => IconBlock.Foreground;
            set => IconBlock.Foreground = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return IconBlock;
        }
    }
}
