using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public class IconSource : MarkupExtension
    {
        private readonly IconChar _icon;
        private ImageSource _imageSource;
        private Brush _foreground = IconHelper.DefaultBrush;

        public Brush Foreground
        {
            get { return _foreground; }
            set
            {
                if (_foreground.Equals(value)) return;
                _foreground = value;
                _imageSource = _icon.ToImageSource(_foreground);
            }
        }


        public IconSource(IconChar icon)
        {
            _icon = icon;
            _imageSource = _icon.ToImageSource(_foreground);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _imageSource;
        }
    }
}
