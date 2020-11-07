using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    /// <summary>
    /// A windows forms dropdown button supporting font awesome icons
    /// </summary>
    public class IconDropDownButton : IconDropDownButton<IconChar>, IHaveIconFont
    {
        public IconDropDownButton() : base(IconChar.Star.FontFamilyFor())
        {
        }

        protected override FontFamily FontFor(IconChar icon)
        {
            return icon.FontFamilyFor(_iconFont);
        }

        private IconFont _iconFont = IconFont.Auto;

        [Category("FontAwesome"), Description("The font awesome style")]
        public IconFont IconFont
        {
            get => _iconFont;
            set
            {
                if (_iconFont.CompareTo(value) == 0) return;
                _iconFont = value;
                UpdateImage();

            }
        }
    }

    /// <summary>
    /// A windows forms dropdown button supporting glyph icons
    /// </summary>
    public class IconDropDownButton<TEnum> : ToolStripDropDownButton, IFormsIcon<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        private readonly FontFamily _fontFamily;
        private Color _color = Color.Black;
        private TEnum _icon;
        private int _size = IconHelper.DefaultSize;
        private FlipOrientation _flip = FlipOrientation.Normal;
        private double _rotation;

        protected IconDropDownButton(FontFamily fontFamily = null)
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum.");
            _fontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
            UpdateImage();
        }

        /// <summary>
        /// Lookup the font to use for the specified icon. Usually only one static font.
        /// Override in case you need special logic.
        /// </summary>
        /// <param name="icon">The icon</param>
        /// <returns>The font for this icon</returns>
        protected virtual FontFamily FontFor(TEnum icon)
        {
            return _fontFamily;
        }

        [Category("FontAwesome"), Description("The icon"), TypeConverter(typeof(IconConverter))]
        public TEnum IconChar
        {
            get => _icon;
            set
            {
                if (_icon.CompareTo(value) == 0) return;
                _icon = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome"), Description("The icon size in pixels"), DefaultValue(FormsIconHelper.DefaultSize)]
        public int IconSize
        {
            get => _size;
            set
            {
                if (_size == value) return;
                _size = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome"), Description("The icon color")]
        public Color IconColor
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome"), Description("The icon flip"), DefaultValue(FlipOrientation.Normal)]
        public FlipOrientation Flip
        {
            get => _flip;
            set
            {
                if (_flip == value) return;
                _flip = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome"), Description("The icon rotation angle in degrees"), DefaultValue(0.0)]
        public double Rotation
        {
            get => _rotation;
            set
            {
                var v = value % 360.0;
                if (Math.Abs(_rotation - v) <= 0.5) return;
                _rotation = v;
                UpdateImage();
            }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        protected void UpdateImage()
        {
            Image = FontFor(_icon).ToBitmap(_icon, _size, _color, _rotation, _flip);
        }

        public bool ShouldSerializeImage()
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/defining-default-values-with-the-shouldserialize-and-reset-methods
            return false;
        }
    }
}
