using System.ComponentModel;
using System.Drawing;

namespace FontAwesome.Sharp
{
    public interface IFormsIcon
    {
        [Category("FontAwesome")]
        IconChar IconChar { get; set; }

        [Category("FontAwesome")]
        int IconSize { get; set; }

        [Category("FontAwesome")]
        Color IconColor { get; set; }

        [Category("Transform")]
        FlipOrientation Flip { get; set; }

        [Category("Transform")]
        double Rotation { get; set; }
    }
}
