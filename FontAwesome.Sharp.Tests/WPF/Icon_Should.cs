using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class Icon_Should
    {
        [StaFact]
        public void Be_Creatable()
        {
            var iconChar = IconChar.Accusoft;
            var markupExtension = new Icon(iconChar);
            markupExtension.Should().NotBeNull();
           // icon.Foreground.ToString().Should().Be("foo");

            var serviceProvider = Substitute.For<IServiceProvider>();
            var iconBlock = markupExtension.ProvideValue(serviceProvider) as IconBlock;
            iconBlock.Should().NotBeNull();
            iconBlock.Icon.Should().Be(iconChar);
        }
    }
}
