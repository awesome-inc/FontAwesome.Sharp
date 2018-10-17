using System.Drawing;

namespace FontAwesome.Sharp
{
    public class IconButton : IconButtonBase<IconChar>
    {
        public IconButton() : base(FormsIconHelper.FontFamilyFor(IconChar.None))
        {
        }

        protected override FontFamily FontFor(IconChar icon)
        {
            return FormsIconHelper.FontFamilyFor(icon);
        }
    }
}
