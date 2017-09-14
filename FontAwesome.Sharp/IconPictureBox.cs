using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FontAwesome.Sharp
{
    public class IconPictureBox : PictureBox
    {
        public static int DefaultIconSize = 32;
        public static new Size DefaultSize = new Size(DefaultIconSize, DefaultIconSize);

        private string _iconText;
        private IconChar _iconChar = IconChar.Star;
        private IconChar lastIconChar = IconChar.Star;
        private int _iconSize = DefaultIconSize;
        private int lasticonSize = DefaultIconSize;
        private Color _BackColor = Color.White;
        private Color lastBgColor;
        private Color _ForeColor = Color.Black;
        private Color lastFontColor;
        private IconFlip _Flip = IconFlip.None;
        private int _Rotation = 0;
        
        /// <summary>
        /// Flip flags
        /// </summary>
        [Flags]
        public enum IconFlip : byte
        {
            /// <summary>
            /// Flip off
            /// </summary>
            None = 0x0,
            /// <summary>
            /// Horizontal flip
            /// </summary>
            Horizontal = 0x1,
            /// <summary>
            /// Vertical flip
            /// </summary>
            Vertical = 0x2,
            /// <summary>
            /// Full flip - same as Rotate(180)
            /// </summary>
            Full = 0x4
        }

        /// <summary>
        /// Icon flip
        /// </summary>
        [Category("Transform"), Description("Flip options")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
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

        /// <summary>
        /// Rotation angle in degrees, ±360°
        /// </summary>
        [Category("Transform"), Description("Rotation angle in degrees, ±360°")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DefaultValue(0)]
        public int Rotation
        {
            get { return _Rotation; }
            set
            {
                if (_Rotation == value) { return; }
                _Rotation = value % 360;
                Invalidate();
            }
        }

        /// <summary>
        /// Hide for constructor Image property
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new Image Image
        {
            get { return base.Image; }
            set { base.Image = value; }
        }
        public bool ShouldSerializeImage()
        {
            return false;
        }

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

        public bool ShouldSerializeIconChar()
        {
            return _iconChar != IconChar.Star;
        }

        [Category("FontAwesome"), Description("If 'SizeMode' is 'Normal', then 'IconSize' depends on Width and Height: `Math.Min(Width, Height)`. In other size modes you can set any value for icon size. ")]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int IconSize
        {
            get
            {
                if (SizeMode == PictureBoxSizeMode.Normal)
                {
                    return Math.Min(base.Width, base.Height);
                }
                else
                {
                    return _iconSize;
                }
            }
            set
            {
                if (value == _iconSize) { return; }
                _iconSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// IconSize serialization only if SizeMode isnt normal
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeIconSize()
        {
            if (SizeMode == PictureBoxSizeMode.Normal)
            {
                return false;
            }
            else
            {
                return _iconSize != DefaultIconSize;

            }
        }

        [DefaultValue(16)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        public new int Width
        {
            get { return base.Width; }
            set
            {
                if (base.Width == value) return;
                base.Width = value;
                Invalidate();
            }
        }

        public bool ShouldSerializeWidth()
        {
            return true;
        }

        [DefaultValue(16)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        public new int Height
        {
            get { return base.Height; }
            set
            {
                if (base.Height == value) return;
                base.Height = value;
                Invalidate();
            }
        }

        public bool ShouldSerializeHeight()
        {
            return true;
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
                return _ForeColor;
            }
            set
            {
                if (_ForeColor == value) { return; }
                _ForeColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Constructor support property
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeForeColor()
        {
            return _ForeColor != Color.Black;
        }

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
                return _BackColor;
            }
            set
            {
                if (_BackColor == value) { return; }
                _BackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Constructor support property
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeBackColor()
        {
            return _BackColor != Color.White;
        }

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
                    (_BackColor == lastBgColor) &
                    (_ForeColor == lastFontColor) &
                    (_iconChar == lastIconChar)
                )
                {
                    return;
                }
                base.Image.Dispose(); // Dispose old image - in other case we will have memory leaks
            }
            
            lasticonSize = _iconSize;
            lastBgColor = _BackColor;
            lastFontColor = _ForeColor;
            lastIconChar = _iconChar;
            
            base.Image = _iconChar.ToBitmap1(IconSize, _ForeColor, _BackColor);
        }

        public IconPictureBox()
        {
            //base.Height = base.Width = DefaultSize;
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
            base.Image.Dispose();
        }

        private void IconPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (SizeMode == PictureBoxSizeMode.Normal)
            {
                IconSize = Math.Min(base.Width, base.Height);
            }
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

        #region Depracated code
        [Obsolete("ActiveColor variable is deprecated, please use ForeColor instead. Thanks!", true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Color ActiveColor
        {
            get { return ForeColor; }
            set { ForeColor = value; }
        }

        [Obsolete("InActiveColor variable is deprecated, please use ForeColor instead. Thanks!", true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Color InActiveColor
        {
            get { return ForeColor; }
            set { ForeColor = value; Invalidate(); }
        }
        #endregion

    }
}