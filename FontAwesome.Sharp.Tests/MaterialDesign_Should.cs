using System;
using FluentAssertions;
using FontAwesome.Sharp.Tests.WindowsForms;
using Xunit;

namespace FontAwesome.Sharp.Tests;

// ReSharper disable once InconsistentNaming
public class MaterialDesign_Should
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

    [Theory]
    [InlineData(typeof(Material.MaterialButton))]
    [InlineData(typeof(Material.MaterialMenuItem))]
    [InlineData(typeof(Material.MaterialPictureBox))]
    [InlineData(typeof(Material.MaterialSplitButton))]
    [InlineData(typeof(Material.MaterialDropDownButton))]
    [InlineData(typeof(Material.MaterialToolStripButton))]
    public void ShouldBeToolboxItem(Type type)
    {
        type.ShouldBeToolboxItem();
    }
}
