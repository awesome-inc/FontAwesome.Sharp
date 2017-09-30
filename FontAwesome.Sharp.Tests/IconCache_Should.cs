using System.Drawing;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests
{
    [TestFixtureFor(typeof(IconCache))]
    // ReSharper disable once InconsistentNaming
    internal class IconCache_Should
    {
        [Test]
        public void Cache_equally_generated_bitmaps()
        {
            using (var sut = new IconCache())
            {
                var bitmap1 = sut.Get(IconChar.AddressBook, 32, Color.Black, Color.Transparent);
                var bitmap2 = sut.Get(IconChar.AddressBook, 32, Color.Black, Color.Transparent);
                bitmap2.Should().BeSameAs(bitmap1, "bitmaps with equal keys should not be recreated");
            }
        }
    }
}
