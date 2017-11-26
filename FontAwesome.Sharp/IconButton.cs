using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconButton : Button, IFormsIcon
    {
        private Color _color = Color.Black;
        private IconChar _icon = IconChar.Star;
        private int _size = 16;
        private FlipOrientation _flip = FlipOrientation.Normal;
        private double _rotation;

        public IconButton()
        {
            UpdateImage();
        }

        [Category("FontAwesome")]
        public IconChar IconChar
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

        [Category("FontAwesome")]
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

        [Category("FontAwesome")]
        public double Rotation
        {
            get => _rotation;
            set
            {
                var v = value % 360.0;
                if (Math.Abs(_rotation - v) < 0.5) return;
                _rotation = v;
                UpdateImage();
            }
        }

        private void UpdateImage()
        {
            Image = _icon.ToBitmap(_size, _color, _rotation, _flip);
        }

        // Dont' serialize image
        // Note: Use DefaultValueAttribute or ShouldSerialize/Reset-methods for a property. Don't use both!
        // cf.: https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/defining-default-values-with-the-shouldserialize-and-reset-methods
        public bool ShouldSerializeImage() { return false; }

        // hide Image in designer(we want only icon)
        [ReadOnly(true)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }
    }
}
