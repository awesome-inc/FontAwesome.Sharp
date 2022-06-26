using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontAwesome.Sharp;

[MarkupExtensionReturnType(typeof(Geometry))]
public abstract class ToGeometryBase<TEnum> : MarkupExtension where TEnum : struct, IConvertible, IComparable, IFormattable
{
    private readonly TEnum _icon;
    private Geometry _geometry;
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Point Origin = new(0,0);

    protected ToGeometryBase(TEnum icon)
    {
        if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum.");
        _icon = icon;
        // ReSharper disable once VirtualMemberCallInConstructor
        UpdateGeometry();
    }

    protected abstract Typeface TypefaceFor(TEnum icon);

    protected void UpdateGeometry()
    {
        var typeface = TypefaceFor(_icon);
#if NET472_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        var dpi = IconHelper.GetDpi();
        var ft = new FormattedText(
            _icon.ToChar(),
            CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight, typeface, 1, IconHelper.DefaultBrush,
            null, TextFormattingMode.Ideal, dpi);
        ft.SetFontSize(IconHelper.DefaultSize / (double)dpi * 96.0);
#else
        var ft = new FormattedText(
            _icon.ToChar(),
            CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight, typeface, 1, IconHelper.DefaultBrush);
#endif
        _geometry = ft.BuildGeometry(Origin);
    }


    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _geometry;
    }
}
