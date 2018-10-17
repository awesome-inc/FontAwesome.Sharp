using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(ToText)), Apartment(ApartmentState.STA)]
    // ReSharper disable once InconsistentNaming
    internal class ToText_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
            var markupExtension = new ToText(iconChar);
            markupExtension.Should().NotBeNull();
            var serviceProvider = Substitute.For<IServiceProvider>();
            var text = markupExtension.ProvideValue(serviceProvider) as string;
            text.Single().Should().Be(iconChar.ToChar());
        }
    }
}