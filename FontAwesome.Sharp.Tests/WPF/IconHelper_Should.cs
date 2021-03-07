using System;
using System.ComponentModel;
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
                var expected = char.ConvertFromUtf32((int)icon);
                icon.ToChar().Should().Be(expected);
            }
        }

        [WpfFact]
        public void Lookup_fonts_for_glyphs()
        {
            foreach (var icon in IconHelper.Icons)
            {
                var font = IconHelper.FontFor(icon);
                font.Should().NotBeNull($"should lookup font for '{icon}'");
                font.Source.Should().Contain("FontAwesome.Sharp");
            }
        }

        [WpfFact]
        public void Generate_imageSources_for_icon_chars()
        {
            foreach (var icon in IconHelper.Icons)
            {
                const int size = 16;
                const int delta = +2;
                var imageSource = icon.ToImageSource(Brushes.Black, size);
                imageSource.Should().NotBeNull($"an image should be generated for '{icon}'");
                imageSource.Should().BeOfType<DrawingImage>();
                Math.Max(imageSource.Width, imageSource.Height).Should().BeLessOrEqualTo(size + delta,
                    $"{icon} size should be less than {size}+{delta}.");
            }
        }

        [WpfFact]
        [Description("Some icons are not contained in any ttf-webfont!")]
        public void Skip_orphaned_icons()
        {
            foreach (var icon in IconHelper.Orphans)
            {
                icon.ToImageSource().Should().BeNull($"{icon} is assumed to be an orphan.");
            }
        }

        [WpfTheory]
        [InlineData(IconChar.Save)]
        public void Support_Font_Styles(IconChar icon)
        {
            var bitmap = icon.ToImageSource(IconFont.Auto, IconHelper.DefaultBrush, 48);
            bitmap.Should().NotBeEmpty();

            bitmap = icon.ToImageSource(IconFont.Regular, IconHelper.DefaultBrush, 48);
            bitmap.Should().NotBeEmpty();
        }
    }
}
