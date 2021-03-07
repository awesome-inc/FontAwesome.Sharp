using System.Drawing;
using System.Windows.Forms;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Xunit;
namespace FontAwesome.Sharp.Tests.WindowsForms
{
    // ReSharper disable once InconsistentNaming
    public class FormsIconHelper_Should
    {

        [StaFact]
        public void Generate_bitmaps_for_icon_chars()
        {
            foreach (var icon in IconHelper.Icons)
            {
                const int size = 16;
                var bitmap = icon.ToBitmap(Color.Black, size);
                bitmap.Should().BeOfType<Bitmap>($"an image should be generated for '{icon}'");
                bitmap.Should().NotBeNull();
                bitmap.Size.Width.Should().Be(size);
                bitmap.Size.Height.Should().Be(size);
            }
        }

        [StaFact]
        public void Add_Icons_to_image_list()
        {
            const int size = 16;
            var imageList = new ImageList();
            imageList.AddIcon(IconChar.AccessibleIcon, Color.Black, size);
            imageList.AddIcons(Color.Black, size,IconChar.AddressBook, IconChar.AirFreshener);
            imageList.Images.Should().HaveCount(3);
        }

        [StaFact]
        public void Support_Font_Styles()
        {
            // see issue #54
            const IconChar brandIcon = IconChar.GoogleDrive;
            var bitmap = brandIcon.ToBitmap(IconFont.Brands);
            bitmap.Should().BeOfType<Bitmap>($"an image should be generated for '{brandIcon}'");
            bitmap.Should().NotBeEmpty();

            bitmap = IconChar.Save.ToBitmap(IconFont.Auto, 256, Color.Black);
            bitmap.Should().NotBeEmpty();

            bitmap = IconChar.Save.ToBitmap(IconFont.Regular, 256, Color.Black);
            bitmap.Should().NotBeEmpty();
        }
    }

    internal static class TestExtensions
    {
        public static BitmapAssertions Should(this Bitmap bitmap)
        {
            return new(bitmap);
        }

        //public static ColorAssertions Should(this Color color)
        //{
        //    return new ColorAssertions(color);
        //}
    }

    /*
    internal class ColorAssertions : ObjectAssertions //ReferenceTypeAssertions<Color, ColorAssertions>
    {
        public ColorAssertions(Color color) : base(color)
        {
        }

        protected override string Identifier => nameof(Color);
    }*/

    internal class BitmapAssertions : ReferenceTypeAssertions<Bitmap, BitmapAssertions>
    {
        public BitmapAssertions(Bitmap bitmap) : base(bitmap)
        {
        }

        protected override string Identifier => nameof(Bitmap);

        public void NotBeEmpty()
        {
            for (var y = 0; y < Subject.Height; y++)
            for (var x = 0; x < Subject.Width; x++)
            {
                var color = Subject.GetPixel(x, y);
                var isEmpty = color.IsEmpty || color.ToArgb() == 0;
                if (!isEmpty)
                    return;
            }
            Execute.Assertion.FailWith("Expected {context} not to be empty, but was.");
        }
    }
}
