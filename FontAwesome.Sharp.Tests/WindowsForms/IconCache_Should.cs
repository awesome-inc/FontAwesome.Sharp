using System.Drawing;
using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    // ReSharper disable once InconsistentNaming
    public class IconCache_Should
    {
        [Fact]
        public void Cache_equally_generated_bitmaps()
        {
            using var sut = new IconCache<IconChar>();
            const IconChar iconChar = IconChar.AddressBook;
            var font = FormsIconHelper.FontFamilyFor(iconChar);
            var bitmap1 = sut.Get(font, iconChar, 32, Color.Black, Color.Transparent);
            var bitmap2 = sut.Get(font, iconChar, 32, Color.Black, Color.Transparent);
            bitmap2.Should().BeSameAs(bitmap1, "bitmaps with equal keys should not be recreated");
        }
    }
}
