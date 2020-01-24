using FluentAssertions;
using Xunit;

namespace TestForms.MaterialDesign
{
    // ReSharper disable once InconsistentNaming
    public class MaterialDesignFont_Should
    {
        [Fact]
        public void Load_Font()
        {
            var font = MaterialDesignFont.FontFamily;
            font.Should().NotBeNull();
            font.Name.Should().Be("Material Design Icons");
        }
    }
}
