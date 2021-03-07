using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class IconImage_Should
    {
        [StaFact]
        public void Be_Creatable()
        {
            var iconChar = IconChar.Accusoft;
            var iconImage = new IconImage { Icon = iconChar, IconFont = IconFont.Auto};
            iconImage.Should().NotBeNull();
            iconImage.Should().BeAssignableTo<IHaveIconFont>();
            iconImage.Icon.Should().Be(iconChar);
            iconImage.IconFont.Should().Be(IconFont.Auto);
        }
    }
}
