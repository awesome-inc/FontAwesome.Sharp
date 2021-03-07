using System;
using System.Windows.Media;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class IconSource_Should
    {
        [WpfTheory]
        [InlineData(IconChar.Accusoft)]
        public void Be_Creatable(IconChar iconChar)
        {
            var iconSource = new IconSource(iconChar);
            iconSource.Should().NotBeNull();
            iconSource.Should().BeAssignableTo<IHaveIconFont>();
            iconSource.IconFont.Should().Be(IconFont.Auto);
            var serviceProvider = Substitute.For<IServiceProvider>();
            var imageSource = (ImageSource)iconSource.ProvideValue(serviceProvider);
            imageSource.Should().NotBeNull();
        }
    }
}
