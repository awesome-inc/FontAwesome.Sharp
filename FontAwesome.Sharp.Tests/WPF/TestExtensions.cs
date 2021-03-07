using System.Windows.Media;
using FluentAssertions.Primitives;

namespace FontAwesome.Sharp.Tests.WPF
{
    internal static class TestExtensions
    {
        public static ImageSourceAssertions Should(this ImageSource imageSource)
        {
            return new(imageSource);
        }

        internal class ImageSourceAssertions : ReferenceTypeAssertions<ImageSource, ImageSourceAssertions>
        {
            public ImageSourceAssertions(ImageSource imageSource) : base(imageSource)
            {
            }

            protected override string Identifier => nameof(ImageSource);

            public void NotBeEmpty()
            {
                NotBeNull();
                //Execute.Assertion.FailWith("Expected {context} not to be empty, but was.");
            }
        }
    }
}
