using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconPictureBox : PictureBox
    {
        /// <summary>
        ///     Default icon char value
        /// </summary>
        public static IconChar DefaultIconChar = IconChar.Star;
        /// <summary>
        ///     Default icon size in pixels
        /// </summary>
        public static int DefaultIconSize = 32;
        /// <summary>
        ///     Default control item size: width and height in pixels
        /// </summary>
        public static new Size DefaultSize = new Size(DefaultIconSize, DefaultIconSize);
        /// <summary>
        ///     Default icon color, RGB
        /// </summary>
        public static new Color DefaultForeColor = Color.Black;
        /// <summary>
        ///     Default background color, ARGB
        /// </summary>
        public static new Color DefaultBackColor = Color.White;
        /// <summary>
        ///     Default icon caching - off or on
        /// </summary>
        public static bool DefaultUseIconCache = false;

        string _iconText;
        IconChar _iconChar = DefaultIconChar;
        IconChar lastIconChar = DefaultIconChar;
        int _iconSize = DefaultIconSize;
        int lasticonSize = DefaultIconSize;
        Color lastBgColor;
        Color lastFontColor;
        IconFlip _Flip = IconFlip.None;
        IconFlip lastFlip = IconFlip.None;
        float _Rotation = 0;
        float lastRotation = 0;
        bool _UseIconCache = DefaultUseIconCache;

        [Category("FontAwesome"), Description("Enable or disable icons caching. Usefull, when you have several controls with same large size icon and you want to save some memory. Also usefull for color change and simple fast animations. Icons is caching by icon, size, 2 colors, rotation, flip.")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseIconCache
        {
            get { return _UseIconCache; }
            set { _UseIconCache = value; }
        }

        public bool ShouldSerializeUseImageCache()
        {
            return _UseIconCache != DefaultUseIconCache;
        }
        public void ResetUseImageCache() { _UseIconCache = DefaultUseIconCache; }

        /// <summary>
        /// Icon flip
        /// </summary>
        [Category("Transform"), Description("Flip options")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(IconFlip.None)]
        public IconFlip Flip
        {
            get { return _Flip; }
            set
            {
                if (_Flip == value) { return; }
                _Flip = value;
                Invalidate();
            }
        }

        public bool ShouldSerializeReset() { return _Flip != IconFlip.None; }
        public void ResetFlip() { Flip = IconFlip.None; }

        /// <summary>
        /// Rotation angle in degrees, ±360°
        /// </summary>
        [Category("Transform"), Description("Rotation angle in degrees, ±360°")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(0)]
        public float Rotation
        {
            get { return _Rotation; }
            set
            {
                var v = value % 360;
                if (_Rotation == v) { return; }
                _Rotation = v;
                Invalidate();
            }
        }

        public bool ShouldSerializeRotation() { return _Rotation != 0; }
        public void ResetRotation() { Rotation = 0; }

        /// <summary>
        /// Hide for constructor Image property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new Image Image
        {
            get { return base.Image; }
            set { base.Image = value; }
        }
        public bool ShouldSerializeImage() { return false; }

        [Category("FontAwesome"), Description("FontAwesome icon")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IconChar IconChar
        {
            get { return _iconChar; }
            set
            {
                _iconChar = value;
                _iconText = char.ConvertFromUtf32((int)_iconChar);
                Invalidate();
            }
        }

        public bool ShouldSerializeIconChar() { return _iconChar != DefaultIconChar; }
        public void ResetIconChar() { IconChar = DefaultIconChar; }

        [Category("FontAwesome"), Description("Can be used only in AutoSize or CenterImage SizeMode. in other modes depends on Width and Height: `Math.Min(Width, Height)`.")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int IconSize
        {
            get
            {
                return _iconSize;
            }
            set
            {
                if (value == _iconSize) { return; }
                _iconSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// IconSize serialization only if SizeMode is auto or center
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeIconSize()
        {
            return _iconSize != DefaultIconSize;
        }

        public void ResetIconSize()
        {
            IconSize = DefaultIconSize;
        }

        /// <summary>
        /// Icon color
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (base.ForeColor == value) { return; }
                base.ForeColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Constructor support property
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeForeColor()
        {
            return base.ForeColor != DefaultBackColor;
        }
        public new void ResetForeColor() { ForeColor = DefaultBackColor; }

        /// <summary>
        /// Back color
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (base.BackColor == value) { return; }
                base.BackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Constructor support property
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeBackColor()
        {
            return base.BackColor != DefaultForeColor;
        }
        public new void ResetBackColor() { ForeColor = DefaultForeColor; }

        /// <summary>
        /// Try to render icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Draw(object sender = null, InvalidateEventArgs e = null)
        {
            if (base.Image != null)
            {
                if (
                    (_iconSize == lasticonSize) &
                    (base.BackColor == lastBgColor) &
                    (base.ForeColor == lastFontColor) &
                    (_iconChar == lastIconChar) &
                    (_Flip == lastFlip) &
                    (_Rotation == lastRotation)
                )
                {
                    return;
                }
                if (!UseIconCache)
                {
                    base.Image.Dispose(); // Dispose old image - in other case we will have memory leaks
                }
            }

            lasticonSize = _iconSize;
            lastBgColor = base.BackColor;
            lastFontColor = base.ForeColor;
            lastIconChar = _iconChar;
            lastFlip = _Flip;
            lastRotation = _Rotation;

            if (UseIconCache)
            {
                base.Image = IconsCache.Get(
                    this,
                    _iconChar,
                    _iconSize,
                    base.ForeColor,
                    base.BackColor,
                    _Flip,
                    _Rotation
                );
            }
            else
            {
                base.Image = _iconChar.ToBitmapGdi(IconSize, base.ForeColor, base.BackColor);
            }
        }

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

        private void IconPictureBox_Disposed(object sender, EventArgs e)
        {
            base.Image = null;  // In some cases I catch errors in forms constructor with image
            IconsCache.Dispose(this);
        }

        private void IconPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (SizeMode != PictureBoxSizeMode.AutoSize)
            {
                IconSize = Math.Min(base.Width, base.Height);
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            Draw();

            // Flip logic
            switch (_Flip)
            {
                case (IconFlip.Horizontal):
                    // Flip the X-Axis
                    graphics.ScaleTransform(-1.0F, 1.0F);
                    // Translate the drawing area accordingly
                    graphics.TranslateTransform(-(float)Width, 0.0F);
                    break;

                case (IconFlip.Vertical):
                    // Flip the Y-Axis
                    graphics.ScaleTransform(1.0F, -1.0F);
                    // Translate the drawing area accordingly
                    graphics.TranslateTransform(0.0F, -(float)Height);
                    break;

                case (IconFlip.Full):
                    graphics.ScaleTransform(-1.0F, -1.0F);
                    graphics.TranslateTransform(-(float)Width, -(float)Height);
                    break;
            }

            // Rotation logic
            if (_Rotation != 0)
            {
                float mx = Width / 2
                    , my = Height / 2
                ;
                graphics.TranslateTransform(mx, my);
                graphics.RotateTransform(_Rotation);
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