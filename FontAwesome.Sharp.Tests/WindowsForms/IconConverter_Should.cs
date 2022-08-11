using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WindowsForms;

// ReSharper disable once InconsistentNaming
public class IconConverter_Should
{
    [Theory]
    [InlineData(typeof(IconChar))]
    public void NotConvertWithDuplicates(Type enumType)
    {
        // see issue https://github.com/awesome-inc/FontAwesome.Sharp/issues/97
        var converter = new IconConverter(enumType);

        var expected = Enum.GetValues(enumType)
            .Cast<object>()
            .DistinctBy(v => v.ToString())
            .OrderBy(v => v.ToString())
            .ToList();
        expected.Should().AllSatisfy(i => i.GetType().Should().Be(enumType));
        expected.GroupBy(i => i.ToString()).Should()
            .AllSatisfy(g => g.Count().Should().Be(1));

        var actual = converter.GetStandardValues();
        actual.Should().BeEquivalentTo(expected);
    }
}
