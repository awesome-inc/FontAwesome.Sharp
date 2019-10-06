using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class IconToImageConverter_Should
    {
        [WpfFact]
        public void Be_Creatable()
        {
            var iconChar = IconChar.Accusoft;
            var converter = new IconToImageConverter();
            converter.Should().NotBeNull();
            var image = (IconImage)converter.Convert(iconChar, null, null, null);
            image.Should().NotBeNull();
            image.Icon.Should().Be(iconChar);
        }
    }
}
