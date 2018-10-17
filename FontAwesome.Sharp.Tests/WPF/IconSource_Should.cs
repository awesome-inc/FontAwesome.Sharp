using System;
using System.Threading;
using System.Windows.Media;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(IconSource)), Apartment(ApartmentState.STA)]
    // ReSharper disable once InconsistentNaming
    internal class IconSource_Should
    {
        [Test]
        [TestCase(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
            var iconSource = new IconSource(iconChar);
            iconSource.Should().NotBeNull();
            var serviceProvider = Substitute.For<IServiceProvider>();
            var imageSource = (ImageSource)iconSource.ProvideValue(serviceProvider);
            imageSource.Should().NotBeNull();
        }
    }
}
