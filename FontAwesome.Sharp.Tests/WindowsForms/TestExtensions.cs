using System;
using System.ComponentModel;
using System.Drawing;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FontAwesome.Sharp.Tests.WindowsForms;

internal static class TestExtensions
{
    public static BitmapAssertions Should(this Bitmap bitmap)
    {
        return new BitmapAssertions(bitmap);
    }

    //public static ColorAssertions Should(this Color color)
    //{
    //    return new ColorAssertions(color);
    //}

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

    public static void ShouldBeToolboxItem<T>() where T : class, IComponent, new()
    {
        typeof(T).ShouldBeToolboxItem();
        // Make custom controls automatically visible in Visual Studio Toolbox, cf.
        // - https://github.com/awesome-inc/FontAwesome.Sharp/issues/62
        // - https://github.com/awesome-inc/FontAwesome.Sharp/issues/91
        // - https://github.com/NuGet/Home/issues/6440#issuecomment-434827530
        // - https://github.com/NuGet/Home/wiki/Converting-Extension-SDKs-into-NuGet-Packages
        // - https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.toolboxitemattribute?view=net-6.0
        typeof(T).Should()
            .BeDecoratedWith<ToolboxItemAttribute>().Which.IsDefaultAttribute().Should().BeTrue();
        typeof(T).Should()
            .BeDecoratedWith<DesignTimeVisibleAttribute>().Which.Visible.Should().BeTrue();
        typeof(T).Should()
            .BeDecoratedWith<DescriptionAttribute>().Which.Description.Should().MatchRegex("A windows forms (.+) supporting font awesome icons");

    }

    public static void ShouldBeToolboxItem(this Type type)
    {
        // Make custom controls automatically visible in Visual Studio Toolbox, cf.
        // - https://github.com/awesome-inc/FontAwesome.Sharp/issues/62
        // - https://github.com/awesome-inc/FontAwesome.Sharp/issues/91
        // - https://github.com/NuGet/Home/issues/6440#issuecomment-434827530
        // - https://github.com/NuGet/Home/wiki/Converting-Extension-SDKs-into-NuGet-Packages
        // - https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.toolboxitemattribute?view=net-6.0
        type.Should().BeAssignableTo<IComponent>();
        type.Should()
            .BeDecoratedWith<ToolboxItemAttribute>().Which.IsDefaultAttribute().Should().BeTrue();
        type.Should()
            .BeDecoratedWith<DesignTimeVisibleAttribute>().Which.Visible.Should().BeTrue();
        type.Should()
            .BeDecoratedWith<DescriptionAttribute>().Which.Description.Should().MatchRegex("A windows forms (?<component>.+) supporting (?<font>.+) icons");
    }
}
