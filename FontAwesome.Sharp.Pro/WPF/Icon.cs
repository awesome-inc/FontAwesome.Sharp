namespace FontAwesome.Sharp.Pro;

public class Icon : IconBase<IconBlock, ProIcons>, IHaveProIconFont
{
    public Icon(ProIcons icon) : base(icon)
    {
    }

    public ProIconFont IconFont
    {
        get => IconBlock.IconFont;
        set => IconBlock.IconFont = value;
    }
}
