using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    [TestFixtureFor(typeof(FormsIconHelper))]
    // ReSharper disable once InconsistentNaming
    internal class FormsIconHelper_Should
    {
        [Test]
        public void Generate_bitmaps_for_icon_chars()
        {
            var limit = -1;
            var icons = Enum.GetValues(typeof(IconChar)).Cast<IconChar>().Skip(1); // 1=None
            if (limit > 0) icons = icons.Take(limit);
            foreach (var icon in icons)
            {
                const int size = 16;
                var bitmap = icon.ToBitmap(size, Color.Black);
                bitmap.Should().BeOfType<Bitmap>($"an image should be generated for '{icon}'");
                bitmap.Should().NotBeNull();
                bitmap.Size.Width.Should().Be(size);
                bitmap.Size.Height.Should().Be(size);
            }
        }

        [Test]
        public void Add_Icons_to_image_list()
        {
            var imageList = new ImageList();
            imageList.AddIcon(IconChar.AccessibleIcon, 16, Color.Black);
            imageList.AddIcons(16, Color.Black, IconChar.AddressBook, IconChar.AirFreshener);
            imageList.Images.Should().HaveCount(3);
        }
    }
}
