using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class IconToImageConverter_Should
    {
        [StaFact]
        public void Be_Creatable()
        {
            var iconChar = IconChar.Accusoft;
            var converter = new IconToImageConverter {IconFont = IconFont.Auto};
            converter.Should().NotBeNull();
            converter.Should().BeAssignableTo<IHaveIconFont>();
            var image = (IconImage)converter.Convert(iconChar, null, null, null);
            image.Should().NotBeNull();
            image.Icon.Should().Be(iconChar);
            image.IconFont.Should().Be(converter.IconFont);
        }
    }
}
