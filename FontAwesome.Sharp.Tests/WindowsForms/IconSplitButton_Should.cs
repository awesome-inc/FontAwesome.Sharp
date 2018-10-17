using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    [TestFixtureFor(typeof(IconSplitButton))]
    // ReSharper disable once InconsistentNaming
    internal class IconSplitButton_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar icon)
        {
            var button = new IconSplitButton { IconChar = icon };
            button.Should().NotBeNull();
            button.IconChar.Should().Be(icon);
        }
    }
}
