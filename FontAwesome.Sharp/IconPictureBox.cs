using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconPictureBox : PictureBox
    {
        private IconChar _iconChar = IconChar.Star;
        private string _tooltip;
        private Color _activeColor = Color.Black;
        private Color _inActiveColor = Color.Black;
        private readonly ToolTip _toolTip = new ToolTip();

        private string _iconText;
        private Font _iconFont;
        private Brush _currentBrush;
        private Brush _activeBrush;
        private Brush _inActiveBrush;

        public IconPictureBox()
            : this(IconChar.Star, 16, Color.DimGray, Color.Black, false, null)
        {
        }

        public IconPictureBox(IconChar icon, int size, Color normalColor, Color hoverColor, bool selectable, string toolTip)
        {
            _iconFont = null;
            // ReSharper disable once VirtualMemberCallInContructor
            BackColor = Color.Transparent;

            // need more than this to make picturebox selectable
            if (selectable)
            {
                SetStyle(ControlStyles.Selectable, true);
                TabStop = true;
            }

            Width = size;
            Height = size;
            IconChar = icon;
            InActiveColor = normalColor;
            ActiveColor = hoverColor;
            ToolTipText = toolTip;

            MouseEnter += Icon_MouseEnter;
            MouseLeave += Icon_MouseLeave;
        }

        private void Icon_MouseLeave(object sender, EventArgs e)
        {
            // change the brush and force a redraw
            _currentBrush = _inActiveBrush;
            Invalidate();
        }

        private void Icon_MouseEnter(object sender, EventArgs e)
        {
            // change the brush and force a redraw
            _currentBrush = _activeBrush;
            Invalidate();
        }

        [Localizable(true)]
        public string ToolTipText
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                if (value == null) return;
                _toolTip.IsBalloon = true;
                _toolTip.ShowAlways = true;
                _toolTip.SetToolTip(this, value);
            }
        }

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

        public Color ActiveColor
        {
            get { return _activeColor; }
            set
            {
                _activeColor = value;
                _activeBrush = new SolidBrush(value);
                Invalidate();
            }
        }

        public Color InActiveColor
        {
            get { return _inActiveColor; }
            set
            {
                _inActiveColor = value;
                _inActiveBrush = new SolidBrush(value);
                _currentBrush = _inActiveBrush;
                Invalidate();
            }
        }

        public new int Width
        {
            get { return base.Width; }
            set
            {
                // force the font size to be recalculated & redrawn
                base.Width = value;
                _iconFont = null;
                Invalidate();
            }
        }

        public new int Height
        {
            get { return base.Height; }
            set
            {
                // force the font size to be recalculated & redrawn
                base.Height = value;
                _iconFont = null;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            // is the font ready to go?
            if (_iconFont == null)
                _iconFont = graphics.GetAdjustedIconFont(_iconText, Width, Height, 4, true);

            graphics.DrawIcon(_iconFont, _iconText, Width, Height, _currentBrush);


            base.OnPaint(e);

            if (!Focused) return;
            var rc = ClientRectangle;
            rc.Inflate(-2, -2);
            ControlPaint.DrawFocusRectangle(e.Graphics, rc);
        }
    }
}