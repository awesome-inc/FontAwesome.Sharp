using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    [TestFixtureFor(typeof(IconPictureBox))]
    // ReSharper disable once InconsistentNaming
    internal class IconPictureBox_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar icon)
        {
            var pictureBox = new IconPictureBox { IconChar = icon };
            pictureBox.Should().NotBeNull();
        }
    }
}
