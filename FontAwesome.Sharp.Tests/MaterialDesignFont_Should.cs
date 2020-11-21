using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests
{
    // ReSharper disable once InconsistentNaming
    public class MaterialDesignFont_Should
    {
        [StaFact]
        public void Load_Fonts()
        {
            var winFormsFont = Material.MaterialDesignFont.WinForms.Value;
            winFormsFont.Should().NotBeNull();
            winFormsFont.Name.Should().Be("Material Design Icons");

            var wpfFont = Material.MaterialDesignFont.Wpf.Value;
            wpfFont.Should().NotBeNull();
            wpfFont.Source.Should().EndWith("Material Design Icons");
        }
    }
}
