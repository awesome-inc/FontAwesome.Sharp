using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    /// <summary>
    /// A windows forms picture box supporting font awesome icons
    /// </summary>
    public class IconPictureBox : IconPictureBox<IconChar>, IHaveIconFont
    {
        public IconPictureBox() : base(IconChar.Star.FontFamilyFor())
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
    /// A windows forms picture box for the specified icon enum type
    /// </summary>
    /// <typeparam name="TEnum">The icon enum</typeparam>
    public abstract class IconPictureBox<TEnum> : PictureBox, IFormsIcon<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        private static readonly IconCache<TEnum> IconCache = new();
        private const int DefaultIconSize = 32;

        // ReSharper disable StaticMemberInGenericType
        public new static readonly Size DefaultSize = new(DefaultIconSize, DefaultIconSize);
        public new static readonly Color DefaultForeColor = Color.Black;
        public new static readonly Color DefaultBackColor = Color.White;
        // ReSharper restore StaticMemberInGenericType

        private readonly FontFamily _fontFamily;
        private TEnum _iconChar;
        private int _iconSize = DefaultIconSize;
        private double _rotation;
        private FlipOrientation _flip = FlipOrientation.Normal;
        private bool _useGdi;
        private bool _useIconCache;

        protected IconPictureBox(FontFamily fontFamily = null)
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum.");
            _fontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
            UpdateImage();

            Size = DefaultSize;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true
            );

            SizeChanged += IconPictureBox_SizeChanged;
            Disposed += IconPictureBox_Disposed;
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

        [Category("FontAwesome")]
        [Description(
            "Enable or disable icons caching. Useful, when you have several controls with same large size icon and you want to save some memory. Also usefull for color change and simple fast animations. Icons is caching by icon, size, 2 colors, rotation, flip.")]
        [DefaultValue(false)]
        public bool UseIconCache
        {
            get => _useIconCache;
            set
            {
                if (_useIconCache == value) return;
                _useIconCache = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        [Description(
            "Enable or disable Gdi rendering. Useful for large icons and enhanced pixel position (However unsafe).")]
        [DefaultValue(false)]
        public bool UseGdi
        {
            get => _useGdi;
            set
            {
                if (_useGdi == value) return;
                _useGdi = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome"), Description("The icon color")]
        public Color IconColor { get => ForeColor; set => ForeColor = value; }

        [Category("FontAwesome"), Description("The icon flip"), DefaultValue(FlipOrientation.Normal)]
        public FlipOrientation Flip
        {
            get => _flip;
            set
            {
                if (_flip == value) return;
                _flip = value;
                Invalidate();
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
                Invalidate();
            }
        }

        [Category("FontAwesome"), Description("The icon"), TypeConverter(typeof(IconConverter))]
        public TEnum IconChar
        {
            get => _iconChar;
            set
            {
                if (_iconChar.CompareTo(value) == 0) return;
                _iconChar = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        [Description(
            "Can be used only in AutoSize or CenterImage SizeMode. in other modes depends on Width and Height: `Math.Min(Width, Height)`.")]
        [DefaultValue(DefaultIconSize)]
        public int IconSize
        {
            get => _iconSize;
            set
            {
                if (value == _iconSize) return;
                _iconSize = value;
                UpdateImage();
            }
        }

        // override fore/back color attributes to make them designer-visible (PictureBox hides them)
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public new Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                if (base.ForeColor == value) return;
                base.ForeColor = value;
                UpdateImage();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public new Color BackColor
        {
            get => base.BackColor;
            set
            {
                if (base.BackColor == value) return;
                base.BackColor = value;
                UpdateImage();
            }
        }

        // Define default attributes for designer serialization.
        // Note: Use DefaultValueAttribute or ShouldSerialize/Reset-methods for a property. Don't use both!
        // cf.: https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/defining-default-values-with-the-shouldserialize-and-reset-methods
        public bool ShouldSerializeImage() { return false; }
        public bool ShouldSerializeForeColor() { return base.ForeColor != DefaultBackColor; }
        public new void ResetForeColor() { ForeColor = DefaultBackColor; }
        public bool ShouldSerializeBackColor() { return base.BackColor != DefaultForeColor; }
        public new void ResetBackColor() { ForeColor = DefaultForeColor; }

        // hide Image in designer (we want only icon)
        [ReadOnly(true), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        protected void UpdateImage()
        {
            var image = base.Image;
            if (image != null && !UseIconCache)
                image.Dispose(); // Dispose old image - in other case we will have memory leaks

            var font = FontFor(_iconChar);
            Image = UseIconCache
                ? IconCache.Get(font, _iconChar, _iconSize, IconColor, BackColor, UseGdi)
                : UseGdi ? font.ToBitmapGdi(_iconChar, IconSize, base.ForeColor, base.BackColor)
                         : font.ToBitmap(_iconChar, IconSize, base.ForeColor);
        }

        private void IconPictureBox_Disposed(object sender, EventArgs e)
        {
            base.Image = null; // In some cases, catch errors in forms constructor with image
        }

        private void IconPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (SizeMode != PictureBoxSizeMode.AutoSize)
                IconSize = Math.Min(Width, Height);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var graphics = pe.Graphics;
            graphics.Flip(Flip, Width, Height);
            graphics.Rotate(Rotation, Width, Height);
            base.OnPaint(pe);

            if (!Focused) return;
            var rc = ClientRectangle;
            rc.Inflate(-2, -2);
            ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
        }
    }
}
