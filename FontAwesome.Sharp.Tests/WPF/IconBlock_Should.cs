using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class IconBlock_Should
    {
        [WpfFact]
        public void Be_Creatable()
        {
            var iconChar = IconChar.Accusoft;
            var iconBlock = new IconBlock { Icon = iconChar };
            iconBlock.Should().NotBeNull();
            iconBlock.Icon.Should().Be(iconChar);
        }
    }
}
