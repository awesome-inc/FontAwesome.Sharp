using FluentAssertions;
using NUnit.Framework;

namespace TestForms.MaterialDesign
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    internal class MaterialDesignFont_Should
    {
        [Test]
        public void Load_Font()
        {
            var font = MaterialDesignFont.FontFamily;
            font.Should().NotBeNull();
            font.Name.Should().Be("Material Design Icons");
        }
    }
}
