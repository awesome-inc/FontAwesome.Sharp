namespace FontAwesome.Sharp.Pro;

public class IconSource : IconSourceBase<ProIcons>
{
    public IconSource(ProIcons icon) : base(icon)
    { }

    protected override void UpdateImageSource()
    {
        ImageSource = ProFonts.WpfFontFor(Icon)
            .ToImageSource(Icon, Foreground, Size);
    }
}
