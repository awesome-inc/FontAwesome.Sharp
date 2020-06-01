using System.Drawing;
using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    public abstract class FormsIconTestBase<T> where T : class, IFormsIcon<IconChar>, new()
    {
        protected virtual bool FlipOnPaint { get; } = false;
        protected virtual bool RotateOnPaint { get; } = false;

        [StaTheory]
        [InlineData(IconChar.Accusoft)]
        public void Be_Creatable(IconChar icon)
        {
            using var formsIcon = new T();
            formsIcon.Should().NotBeNull();
            formsIcon.IconChar = icon;
            formsIcon.IconChar.Should().Be(icon);
            formsIcon.Image.Should().NotBeNull();
            formsIcon.ShouldSerializeImage().Should()
                .BeFalse("Image should not be serialized in .resx since generated");
        }

        [StaFact]
        public void Skip_small_Rotation_changes()
        {
            using var formsIcon = new T();
            formsIcon.Rotation.Should().Be(0);

            formsIcon.Rotation = 0.5;
            formsIcon.Rotation.Should().Be(0);

            formsIcon.Rotation = 1.0;
            formsIcon.Rotation.Should().Be(1);
        }

        [StaFact]
        public void Update_image_on_changes()
        {
            using var formsIcon = new T();
            // icon
            var image = formsIcon.Image;
            formsIcon.IconChar = IconChar.Accusoft;
            formsIcon.Image.Should().NotBe(image);

            // size
            formsIcon.IconSize = 16;
            image = formsIcon.Image;
            formsIcon.IconSize.Should().Be(16);
            formsIcon.IconSize = 12;
            formsIcon.Image.Should().NotBe(image);

            // color
            formsIcon.IconColor = Color.AliceBlue;
            image = formsIcon.Image;
            formsIcon.IconColor.Should().Be(Color.AliceBlue);
            formsIcon.IconColor = Color.Black;
            formsIcon.Image.Should().NotBe(image);

            // flip
            formsIcon.Flip = FlipOrientation.Normal;
            image = formsIcon.Image;
            formsIcon.Flip.Should().Be(FlipOrientation.Normal);
            formsIcon.Flip = FlipOrientation.Horizontal;
            if (!FlipOnPaint) formsIcon.Image.Should().NotBe(image);

            // rotation
            image = formsIcon.Image;
            formsIcon.Rotation.Should().Be(0);
            formsIcon.Rotation = 22.5;
            if (!RotateOnPaint) formsIcon.Image.Should().NotBe(image);
        }
    }
}
