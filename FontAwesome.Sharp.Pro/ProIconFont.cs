namespace FontAwesome.Sharp.Pro;

public enum ProIconFont
{
    /// <summary>
    /// Auto detect
    /// </summary>
    Auto,
    /// <summary>
    /// Regular style
    /// </summary>
    Regular,
    /// <summary>
    /// Solid style
    /// </summary>
    Solid,
    /// <summary>
    /// Light Style
    /// </summary>
    Light,
    /// <summary>
    /// Light Style
    /// </summary>
    Thin,
}

public interface IHaveProIconFont
{
    ProIconFont IconFont { get; }
}
