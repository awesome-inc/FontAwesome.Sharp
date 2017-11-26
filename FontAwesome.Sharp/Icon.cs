using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public class Icon : MarkupExtension
    {
        private readonly IconBlock _iconBlock;

        public Icon(IconChar icon)
        {
            _iconBlock = new IconBlock
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