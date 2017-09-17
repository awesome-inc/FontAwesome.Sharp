using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconPictureBox : PictureBox
    {
        public const int DefaultIconSize = 32;
        public new static Size DefaultSize = new Size(DefaultIconSize, DefaultIconSize);
        public new static Color DefaultForeColor = Color.Black;
        public new static Color DefaultBackColor = Color.White;
        public static bool DefaultUseIconCache = false;
        private Color _backColor = Color.White;
        private IconFlip _flip = IconFlip.None;
        private Color _foreColor = Color.Black;
        private IconChar _iconChar = IconChar.Star;
        private int _iconSize = DefaultIconSize;

        private int _rotation;

        private static readonly IconCache IconCache = new IconCache();
        private Color _lastBgColor;
        private IconFlip _lastFlip = IconFlip.None;
        private Color _lastFontColor;
        private IconChar _lastIconChar = IconChar.Star;
        private int _lasticonSize = DefaultIconSize;
        private int _lastRotation;

        public IconPictureBox()
        {
            Size = DefaultSize;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true
            );

            Invalidated += Draw;
            SizeChanged += IconPictureBox_SizeChanged;
            Disposed += IconPictureBox_Disposed;
            Draw();
        }

        [Category("FontAwesome")]
        [Description(
            "Enable or disable icons caching. Usefull, when you have several controls with same large size icon and you want to save some memory. Also usefull for color change and animations. Icons caching by icon, size, 2 colors, rotation, flip.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseIconCache { get; set; } = DefaultUseIconCache;

        /// <summary>
        ///     Icon flip
        /// </summary>
        [Category("Transform")]
        [Description("Flip options")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(IconFlip.None)]
        public IconFlip Flip
        {
            get => _flip;
            set
            {
                if (_flip == value) return;
                _flip = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Rotation angle in degrees, ±360°
        /// </summary>
        [Category("Transform")]
        [Description("Rotation angle in degrees, ±360°")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(0)]
        public int Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation == value) return;
                _rotation = value % 360;
                Invalidate();
            }
        }

        /// <summary>
        ///     Hide for constructor Image property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        [Category("FontAwesome")]
        [Description("FontAwesome icon")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IconChar IconChar
        {
            get => _iconChar;
            set
            {
                _iconChar = value;
                char.ConvertFromUtf32((int) _iconChar);
                Invalidate();
            }
        }

        [Category("FontAwesome")]
        [Description(
            "If 'SizeMode' is 'Normal', then 'IconSize' depends on Width and Height: `Math.Min(Width, Height)`. In other size modes you can set any value for icon size. ")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int IconSize
        {
            get => SizeMode == PictureBoxSizeMode.Normal ? Math.Min(base.Width, base.Height) : _iconSize;
            set
            {
                if (value == _iconSize) return;
                _iconSize = value;
                Invalidate();
            }
        }

        [DefaultValue(DefaultIconSize)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public new int Width
        {
            get => base.Width;
            set
            {
                if (base.Width == value) return;
                base.Width = value;
                Invalidate();
            }
        }

        [DefaultValue(DefaultIconSize)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public new int Height
        {
            get => base.Height;
            set
            {
                if (base.Height == value) return;
                base.Height = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Icon color
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor == value) return;
                _foreColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Back color
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value) return;
                _backColor = value;
                Invalidate();
            }
        }

        public bool ShouldSerializeUseImageCache()
        {
            return UseIconCache != DefaultUseIconCache;
        }

        public void ResetUseImageCache()
        {
            UseIconCache = DefaultUseIconCache;
        }

        public bool ShouldSerializeReset()
        {
            return _flip != IconFlip.None;
        }

        public void ResetFlip()
        {
            Flip = IconFlip.None;
        }

        public bool ShouldSerializeRotation()
        {
            return _rotation != 0;
        }

        public void ResetRotation()
        {
            Rotation = 0;
        }

        public bool ShouldSerializeImage()
        {
            return false;
        }

        public bool ShouldSerializeIconChar()
        {
            return _iconChar != IconChar.Star;
        }

        public void ResetIconChar()
        {
            IconChar = IconChar.Star;
        }

        /// <summary>
        ///     IconSize serialization only if SizeMode isnt normal
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeIconSize()
        {
            if (SizeMode == PictureBoxSizeMode.Normal)
                return false;
            return _iconSize != DefaultIconSize;
        }

        public void ResetIconSize()
        {
            if (SizeMode != PictureBoxSizeMode.Normal)
                IconSize = DefaultIconSize;
        }

        public bool ShouldSerializeWidth()
        {
            return true;
        }

        public void ResetWidth()
        {
            Width = DefaultSize.Width;
        }

        public bool ShouldSerializeHeight()
        {
            return true;
        }

        public void ResetHeight()
        {
            Height = DefaultSize.Height;
        }

        /// <summary>
        ///     Constructor support property
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeForeColor()
        {
            return _foreColor != DefaultBackColor;
        }

        public new void ResetForeColor()
        {
            ForeColor = DefaultBackColor;
        }

        /// <summary>
        ///     Constructor support property
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeBackColor()
        {
            return _backColor != DefaultForeColor;
        }

        public new void ResetBackColor()
        {
            ForeColor = DefaultForeColor;
        }

        /// <summary>
        ///     Try to render icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Draw(object sender = null, InvalidateEventArgs e = null)
        {
            if (base.Image != null)
            {
                if (
                    (_iconSize == _lasticonSize) &
                    (_backColor == _lastBgColor) &
                    (_foreColor == _lastFontColor) &
                    (_iconChar == _lastIconChar) &
                    (_flip == _lastFlip) &
                    (_rotation == _lastRotation)
                )
                    return;
                if (!UseIconCache)
                    base.Image.Dispose(); // Dispose old image - in other case we will have memory leaks
            }

            _lasticonSize = _iconSize;
            _lastBgColor = _backColor;
            _lastFontColor = _foreColor;
            _lastIconChar = _iconChar;
            _lastFlip = _flip;
            _lastRotation = _rotation;

            base.Image = UseIconCache 
                ? IconCache[_iconChar, IconSize, _foreColor, _backColor, _flip, _rotation] 
                : _iconChar.ToBitmapGdi(IconSize, _foreColor, _backColor);
        }

        private void IconPictureBox_Disposed(object sender, EventArgs e)
        {
            base.Image.Dispose();
        }

        private void IconPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (SizeMode == PictureBoxSizeMode.Normal)
                IconSize = Math.Min(base.Width, base.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            Draw();

            // Flip logic
            switch (_flip)
            {
                case IconFlip.Horizontal:
                    // Flip the X-Axis
                    graphics.ScaleTransform(-1f, 1f);
                    // Translate the drawing area accordingly
                    graphics.TranslateTransform(-Width, 0f);
                    break;

                case IconFlip.Vertical:
                    // Flip the Y-Axis
                    graphics.ScaleTransform(1f, -1f);
                    // Translate the drawing area accordingly
                    graphics.TranslateTransform(0.0F, -Height);
                    break;

                case IconFlip.Full:
                    graphics.ScaleTransform(-1f, -1f);
                    graphics.TranslateTransform(-Width, -Height);
                    break;
            }

            // Rotation logic
            if (_rotation != 0)
            {
                var mx = 0.5f * Width;
                var my = 0.5f * Height;
                graphics.TranslateTransform(mx, my);
                graphics.RotateTransform(_rotation);
                graphics.TranslateTransform(-mx, -my);
            }

            base.OnPaint(e);

            if (!Focused) return;
            var rc = ClientRectangle;
            rc.Inflate(-2, -2);
            ControlPaint.DrawFocusRectangle(e.Graphics, rc);
        }
    }
}