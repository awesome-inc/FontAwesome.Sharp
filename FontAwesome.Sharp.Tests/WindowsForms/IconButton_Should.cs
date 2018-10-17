using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    [TestFixtureFor(typeof(IconButton))]
    // ReSharper disable once InconsistentNaming
    internal class IconButton_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar icon)
        {
            var button = new IconButton { IconChar = icon };
            button.Should().NotBeNull();
            button.IconChar.Should().Be(icon);
        }
    }
}
