using System;
using System.Threading;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(Icon)), Apartment(ApartmentState.STA)]
    // ReSharper disable once InconsistentNaming
    internal class Icon_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
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
