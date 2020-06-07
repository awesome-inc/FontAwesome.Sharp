namespace FontAwesome.Sharp
{
    public class IconSource : IconSourceBase<IconChar>
    {
        public IconSource(IconChar icon) : base(icon)
        {
        }

        protected override void UpdateImageSource()
        {
            ImageSource = Icon.ToImageSource(Foreground, Size);
        }
    }
}
