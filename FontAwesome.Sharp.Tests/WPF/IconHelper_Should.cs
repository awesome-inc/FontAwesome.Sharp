using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class IconHelper_Should
    {
        [Fact]
        public void Convert_icon_enums_to_characters()
        {
            foreach (var icon in IconHelper.Icons)
            {
                var expected = char.ConvertFromUtf32((int)icon).Single();
                icon.ToChar().Should().Be(expected);
            }
        }

        [Fact]
        public void Lookup_fonts_for_glyphs()
        {
            foreach (var icon in IconHelper.Icons)
            {
                var fontFamily = IconHelper.FontFor(icon);
                fontFamily.Should().NotBeNull($"should lookup font for '{icon}'");
                fontFamily.Source.Should().Contain("FontAwesome.Sharp");
            }
        }

        [Fact]
        public void Generate_imageSources_for_icon_chars()
        {
            foreach (var icon in IconHelper.Icons)
            {
                const int size = 16;
                const int delta = +2;
                var imageSource = icon.ToImageSource(Brushes.Black, size);
                imageSource.Should().NotBeNull($"an image should be generated for '{icon}'");
                imageSource.Should().BeOfType<DrawingImage>();
                Math.Max(imageSource.Width, imageSource.Height).Should().BeLessOrEqualTo(size+delta, $"{icon} size should be less than {size}+{delta}.");
            }
        }

        [Fact]
        [Description("Some icons are not contained in any ttf-webfont!")]
        public void Skip_orphaned_icons()
        {
            foreach(var icon in IconHelper.Orphans)
            {
                icon.ToImageSource().Should().BeNull($"{icon} is assumed to be an orphan.");
            }
        }

    }
}
