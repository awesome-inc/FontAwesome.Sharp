using System;
using System.Linq;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FontAwesome.Sharp.Tests
{
    [TestFixtureFor(typeof(IconHelper))]
    // ReSharper disable once InconsistentNaming
    internal class IconHelper_Should
    {
        [Test]
        public void Convert_icon_enums_to_characters()
        {
            foreach (var icon in Enum.GetValues(typeof(IconChar)).Cast<IconChar>())
            {
                var expected = char.ConvertFromUtf32((int)icon).Single();
                icon.ToChar().Should().Be(expected);
            }
        }
    }
}
