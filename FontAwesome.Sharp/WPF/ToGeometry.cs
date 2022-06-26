using System.Windows.Media;

namespace FontAwesome.Sharp;

public class ToGeometry : ToGeometryBase<IconChar>
{
    private IconFont _iconFont = IconFont.Auto;

    public ToGeometry(IconChar icon) : base(icon)
    {
    }

    public IconFont IconFont
    {
        get => _iconFont;
        set
        {
            if (_iconFont.Equals(value)) return;
            _iconFont = value;
            UpdateGeometry();
        }
    }

    protected override Typeface TypefaceFor(IconChar icon)
    {
        return IconHelper.TypefaceFor(icon, _iconFont);
    }
}
