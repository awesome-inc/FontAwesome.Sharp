using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.WPF
{
    public class Icon : MarkupExtension
    {
        private readonly IconBlock _iconBlock;

        public Brush Foreground
        {
            get { return _iconBlock.Foreground; }
            set { _iconBlock.Foreground = value; }
        }

        public Icon(IconChar icon)
        {
            _iconBlock = new IconBlock { Icon = icon };
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _iconBlock;
        }
    }
}