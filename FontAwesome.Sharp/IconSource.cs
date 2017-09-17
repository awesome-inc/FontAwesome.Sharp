using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public class IconSource : MarkupExtension
    {
        private readonly IconChar _icon;
        private Brush _foreground = IconHelper.DefaultBrush;
        private ImageSource _imageSource;
        private double _size = IconHelper.DefaultSize;


        public IconSource(IconChar icon)
        {
            _icon = icon;
            _imageSource = _icon.ToImageSource(_foreground, _size);
        }

        public Brush Foreground
        {
            get => _foreground;
            set
            {
                if (_foreground.Equals(value)) return;
                _foreground = value;
                UpdateImageSource();
            }
        }

        public double Size
        {
            get => _size;
            set
            {
                if (Math.Abs(_size - value) < 0.5) return;
                _size = value;
                UpdateImageSource();
            }
        }

        private void UpdateImageSource()
        {
            _imageSource = _icon.ToImageSource(_foreground, _size);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _imageSource;
        }
    }
}