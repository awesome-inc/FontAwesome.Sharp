using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    [TestFixtureFor(typeof(IconMenuItem))]
    // ReSharper disable once InconsistentNaming
    internal class IconMenuItem_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar icon)
        {
            var menuItem = new IconMenuItem { IconChar = icon };
            menuItem.Should().NotBeNull();
            menuItem.IconChar.Should().Be(icon);
        }
    }
}
