using System.Threading;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(IconImage)), Apartment(ApartmentState.STA)]
    // ReSharper disable once InconsistentNaming
    internal class IconImage_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
            var iconImage = new IconImage { Icon = iconChar };
            iconImage.Should().NotBeNull();
            iconImage.Icon.Should().Be(iconChar);
        }
    }
}
