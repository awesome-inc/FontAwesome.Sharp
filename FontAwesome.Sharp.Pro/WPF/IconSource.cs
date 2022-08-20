namespace FontAwesome.Sharp.Pro;

public class IconSource : IconSourceBase<ProIcons>, IHaveProIconFont
{
    private ProIconFont _iconFont = ProIconFont.Auto;

    public IconSource(ProIcons icon) : base(icon)
    { }

    protected override void UpdateImageSource()
    {
        ImageSource = Icon.WpfFontFor(_iconFont)
            .ToImageSource(Icon, Foreground, Size);
    }

    public ProIconFont IconFont
    {
        get => _iconFont;
        set
        {
            if (_iconFont.Equals(value)) return;
            _iconFont = value;
            UpdateImageSource();
        }
    }

}
