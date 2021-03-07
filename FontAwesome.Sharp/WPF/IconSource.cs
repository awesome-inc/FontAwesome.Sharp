namespace FontAwesome.Sharp
{
    public class IconSource : IconSourceBase<IconChar>, IHaveIconFont
    {
        private IconFont _iconFont = IconFont.Auto;

        public IconSource(IconChar icon) : base(icon)
        {
        }

        protected override void UpdateImageSource()
        {
            ImageSource = Icon.ToImageSource(IconFont, Foreground, Size);
        }

        public IconFont IconFont
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
}
