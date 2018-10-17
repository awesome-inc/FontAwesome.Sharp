using System;
using System.Linq;
using System.Windows.Media;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WPF
{
    [TestFixtureFor(typeof(IconHelper))]
    // ReSharper disable once InconsistentNaming
    internal class IconHelper_Should
    {
        [Test]
        public void Convert_icon_enums_to_characters()
        {
            foreach (var icon in Enum.GetValues(typeof(IconChar)).Cast<IconChar>())
            {
                var expected = char.ConvertFromUtf32((int)icon).Single();
                icon.ToChar().Should().Be(expected);
            }
        }

        [Test]
        public void Lookup_fonts_for_glyphs()
        {
            var limit = -1;
            var icons = Enum.GetValues(typeof(IconChar)).Cast<IconChar>().Skip(1); // 1=None
            if (limit > 0) icons = icons.Take(limit);
            foreach (var icon in icons)
            {
                var fontFamily = IconHelper.FontFor(icon);
                fontFamily.Should().NotBeNull($"should lookup font for '{icon}'");
                fontFamily.Source.Should().Contain("FontAwesome.Sharp");
            }
        }

        [Test]
        public void Generate_imageSources_for_icon_chars()
        {
            var limit = -1;
            var icons = Enum.GetValues(typeof(IconChar)).Cast<IconChar>().Skip(1); // 1=None
            if (limit > 0) icons = icons.Take(limit);
            foreach (var icon in icons)
            {
                const int size = 16;
                var imageSource = icon.ToImageSource(Brushes.Black, size);
                imageSource.Should().NotBeNull($"an image should be generated for '{icon}'");
                imageSource.Should().BeOfType<DrawingImage>();
                Math.Round(imageSource.Width).Should().BeLessOrEqualTo(size + 1);
                Math.Round(imageSource.Height).Should().BeLessOrEqualTo(size + 1);
            }
        }

    }
}
