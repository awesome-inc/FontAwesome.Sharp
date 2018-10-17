using System.Drawing;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests
{
    [TestFixtureFor(typeof(IconCache<IconChar>))]
    // ReSharper disable once InconsistentNaming
    internal class IconCache_Should
    {
        [Test]
        public void Cache_equally_generated_bitmaps()
        {
            using (var sut = new IconCache<IconChar>())
            {
                const IconChar iconChar = IconChar.AddressBook;
                var font = FormsIconHelper.FontFamilyFor(iconChar);
                var bitmap1 = sut.Get(font, iconChar, 32, Color.Black, Color.Transparent);
                var bitmap2 = sut.Get(font, iconChar, 32, Color.Black, Color.Transparent);
                bitmap2.Should().BeSameAs(bitmap1, "bitmaps with equal keys should not be recreated");
            }
        }
    }
}
