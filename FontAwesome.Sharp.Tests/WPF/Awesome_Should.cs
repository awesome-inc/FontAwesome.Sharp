using System.Linq;
using System.Windows.Documents;
using FluentAssertions;
using Xunit;

namespace FontAwesome.Sharp.Tests.WPF
{
    // ReSharper disable once InconsistentNaming
    public class Awesome_Should
    {
        [WpfTheory]
        [InlineData(@":(\w+):", ":Bitcoin: is a cryptocurrency. :EuroSign: is a fiat money.")]
        [InlineData(@"{fa:(\w+)}", "{fa:Bitcoin} is a cryptocurrency. {fa:EuroSign} is a fiat money.")]
        public void Format_Text(string pattern, string input)
        {
            var runs = Awesome.FormatText(input, pattern).OfType<Run>().ToList();

            runs.Should().HaveCount(4);
            AssertIcon(runs[0].Text, IconChar.Bitcoin);
            runs[1].Text.Should().Be(" is a cryptocurrency. ");
            AssertIcon(runs[2].Text, IconChar.EuroSign);
            runs[3].Text.Should().Be(" is a fiat money.");
        }

        private static void AssertIcon(string text, IconChar icon)
        {
            ((int) text.Single()).Should().Be((int) icon);
        }
    }
}
