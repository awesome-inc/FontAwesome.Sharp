using System;
using System.Drawing;

namespace FontAwesome.Sharp
{
    public interface IFormsIcon<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        TEnum IconChar { get; set; }
        int IconSize { get; set; }
        Color IconColor { get; set; }
        FlipOrientation Flip { get; set; }
        double Rotation { get; set; }
    }
}
