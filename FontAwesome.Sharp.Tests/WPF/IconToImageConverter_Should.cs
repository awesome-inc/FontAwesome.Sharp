using System.Threading;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(IconToImageConverter)), Apartment(ApartmentState.STA)]
    // ReSharper disable once InconsistentNaming
    internal class IconToImageConverter_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
            var converter = new IconToImageConverter();
            converter.Should().NotBeNull();
            var image = (IconImage)converter.Convert(iconChar, null, null, null);
            image.Should().NotBeNull();
            image.Icon.Should().Be(iconChar);
        }
    }
}
