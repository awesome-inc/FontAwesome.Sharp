using System;
using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Sharp.Pro;

public class IconImage : IconImageBase<ProIcons>, IHaveProIconFont
{
    protected override ImageSource ImageSourceFor(ProIcons icon)
    {
        var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
        return icon.WpfFontFor(IconFont).ToImageSource(icon, Foreground, size);
    }

    public static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont),
        typeof(IconImage), typeof(IconImage),
        new PropertyMetadata(ProIconFont.Auto, OnIconFontPropertyChanged));

    private static void OnIconFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not IconImage iconImage) return;
        var imageSource = iconImage.ImageSourceFor(iconImage.Icon);
        iconImage.SetValue(SourceProperty, imageSource);
    }

    public ProIconFont IconFont {
        get => (ProIconFont)GetValue(IconFontProperty);
        set => SetValue(IconFontProperty, value);
    }
}
