using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    // ReSharper disable once InconsistentNaming
    public class IconPictureBox_Should : FormsIconTestBase<IconPictureBox>
    {
        protected override bool FlipOnPaint { get; } = true;
        protected override bool RotateOnPaint { get; } = true;

        [StaTheory]
        [Description("FontAwesome v5.8.2 icons not displaying, cf.: https://github.com/awesome-inc/FontAwesome.Sharp/issues/20")]
        [InlineData(IconChar.AngleDoubleLeft)]
        [InlineData(IconChar.Stackpath)]
        public void Display_Icons(IconChar icon)
        {
            using var pictureBox = new IconPictureBox { IconChar = icon };
            pictureBox.Image.Should().NotBeNull();
        }
    }
}
