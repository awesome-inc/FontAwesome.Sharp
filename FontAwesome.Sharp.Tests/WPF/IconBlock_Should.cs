using System.Threading;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(IconBlock)), Apartment(ApartmentState.STA)]
    // ReSharper disable once InconsistentNaming
    internal class IconBlock_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
            var iconBlock = new IconBlock { Icon = iconChar };
            iconBlock.Should().NotBeNull();
            iconBlock.Icon.Should().Be(iconChar);
        }
    }
}
