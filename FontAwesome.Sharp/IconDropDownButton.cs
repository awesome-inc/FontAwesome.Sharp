using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconDropDownButton : ToolStripDropDownButton, IFormsIcon
    {
        private Color _color = Color.Black;
        private IconChar _icon = IconChar.Star;
        private int _size = 16;

        public IconDropDownButton()
        {
            UpdateImage();
        }

        [Category("FontAwesome")]
        public IconChar Icon
        {
            get => _icon;
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
            get => _size;
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
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                UpdateImage();
            }
        }

        // TODO: implement
        public IconFlip Flip { get; set; }
        // TODO: implement
        public int Rotation { get; set; }

        private void UpdateImage()
        {
            Image = _icon.ToBitmap(_size, _color);
        }
    }
}