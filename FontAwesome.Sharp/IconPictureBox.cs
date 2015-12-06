using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconPictureBox : PictureBox
    {
        private IconChar _iconChar = IconChar.Star;
        private Color _activeColor = Color.Black;
        private Color _inActiveColor = Color.Black;

        private string _iconText;
        private Font _iconFont;
        private Brush _currentBrush;
        private Brush _activeBrush;
        private Brush _inActiveBrush;

        public IconPictureBox()
        {
            // ReSharper disable once VirtualMemberCallInContructor
            BackColor = Color.Transparent;
            Width = Height = 16;
            MouseEnter += Icon_MouseEnter;
            MouseLeave += Icon_MouseLeave;
        }

        [Category("FontAwesome")]
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

        [Category("FontAwesome")]
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

        [Category("FontAwesome")]
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
                if (base.Width == value) return;
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
                if (base.Height == value) return;
                base.Height = value;
                _iconFont = null;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            if (_iconFont == null)
                _iconFont = graphics.GetAdjustedIconFont(_iconText, Width, Height, 4, true);

            graphics.DrawIcon(_iconFont, _iconText, Width, Height, _currentBrush);

            base.OnPaint(e);

            if (!Focused) return;
            var rc = ClientRectangle;
            rc.Inflate(-2, -2);
            ControlPaint.DrawFocusRectangle(e.Graphics, rc);
        }

        private void Icon_MouseLeave(object sender, EventArgs e)
        {
            _currentBrush = _inActiveBrush;
            Invalidate();
        }

        private void Icon_MouseEnter(object sender, EventArgs e)
        {
            _currentBrush = _activeBrush;
            Invalidate();
        }
    }
}