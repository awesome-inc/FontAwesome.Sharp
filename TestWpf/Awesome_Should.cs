using System.Linq;
using System.Windows.Documents;
using FluentAssertions;
using FontAwesome.Sharp;
using NEdifis.Attributes;
using NUnit.Framework;

namespace TestWpf
{
    [TestFixtureFor(typeof (Awesome))]
    // ReSharper disable once InconsistentNaming
    internal class Awesome_Should
    {
        [Test]
        [TestCase(@":(\w+):", ":btc: is a cryptocurrency. :eur: is a fiat money.")]
        [TestCase(@"{fa:(\w+)}", "{fa:Btc} is a cryptocurrency. {fa:Eur} is a fiat money.")]
        public void Format_Text(string pattern, string input)
        {
            var runs = Awesome.FormatText(input, pattern).OfType<Run>().ToList();

            runs.Should().HaveCount(4);
            AssertIcon(runs[0].Text, IconChar.Btc);
            runs[1].Text.Should().Be(" is a cryptocurrency. ");
            AssertIcon(runs[2].Text, IconChar.Eur);
            runs[3].Text.Should().Be(" is a fiat money.");
        }

        private static void AssertIcon(string text, IconChar icon)
        {
            ((int) text.Single()).Should().Be((int) icon);
        }
    }
}