using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp
{
    public abstract class IconSourceBase<TEnum> : MarkupExtension
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        protected readonly TEnum Icon;
        protected ImageSource ImageSource;
        private Brush _foreground = IconHelper.DefaultBrush;
        private double _size = IconHelper.DefaultSize;

        protected abstract void UpdateImageSource();

        protected IconSourceBase(TEnum icon)
        {
            Icon = icon;
            // ReSharper disable once VirtualMemberCallInConstructor
            UpdateImageSource();
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

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ImageSource;
        }
    }
}
