using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    [TestFixtureFor(typeof(IconToolStripButton))]
    // ReSharper disable once InconsistentNaming
    internal class IconToolStripButton_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar icon)
        {
            var button = new IconToolStripButton { IconChar = icon };
            button.Should().NotBeNull();
            button.IconChar.Should().Be(icon);
        }
    }
}
