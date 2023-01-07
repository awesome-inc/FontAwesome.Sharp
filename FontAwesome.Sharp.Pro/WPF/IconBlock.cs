using System.Windows;
using System.Windows.Media;

namespace FontAwesome.Sharp.Pro;

public class IconBlock : IconBlockBase<ProIcons>, IHaveProIconFont
{
    public IconBlock() : base(ProIcons.Abacus.WpfFontFor())
    {
    }

    protected override FontFamily FontFor(ProIcons icon)
    {
        return icon.WpfFontFor(IconFont);
    }

    public static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont),
        typeof(ProIconFont), typeof(IconBlock),
        new PropertyMetadata(ProIconFont.Auto, OnIconFontPropertyChanged));

    private static void OnIconFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not IconBlock iconBlock) return;
        iconBlock.SetValue(FontFamilyProperty, iconBlock.FontFor(iconBlock.Icon));
    }

    public ProIconFont IconFont {
        get => (ProIconFont)GetValue(IconFontProperty);
        set => SetValue(IconFontProperty, value);
    }
}
