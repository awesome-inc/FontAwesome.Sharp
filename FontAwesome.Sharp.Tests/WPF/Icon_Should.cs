using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class Icon_Should
    {
        [WpfFact]
        public void Be_Creatable()
        {
            var iconChar = IconChar.Accusoft;
            var icon = new Icon(iconChar);
            icon.Should().NotBeNull();
           // icon.Foreground.ToString().Should().Be("foo");
           icon.Should().BeAssignableTo<IHaveIconFont>();
           icon.IconFont.Should().Be(IconFont.Auto);

            var serviceProvider = Substitute.For<IServiceProvider>();
            var iconBlock = icon.ProvideValue(serviceProvider) as IconBlock;
            iconBlock.Should().NotBeNull();
            iconBlock.Icon.Should().Be(iconChar);
        }
    }
}
