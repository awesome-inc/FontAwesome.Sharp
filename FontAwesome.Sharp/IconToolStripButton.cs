using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconToolStripButton : ToolStripButton, IFormsIcon
    {
        private IconChar _icon = IconChar.Star;
        private Color _color = Color.Black;
        private int _size = 16;

        public IconToolStripButton()
        {
            UpdateImage();
        }

        [Category("FontAwesome")]
        public IconChar Icon
        {
            get { return _icon; }
            set
            {
                if (_icon == value) return;
                _icon = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        public int IconSize
        {
            get { return _size; }
            set
            {
                if (_size == value) return;
                _size = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        public Color IconColor
        {
            get { return _color; }
            set
            {
                if (_color == value) return;
                _color = value;
                UpdateImage();
            }
        }

        private void UpdateImage()
        {
            Image = _icon.ToBitmap(_size, _color);
        }
    }
}