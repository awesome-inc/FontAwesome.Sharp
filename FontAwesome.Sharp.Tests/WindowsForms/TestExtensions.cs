using System.Drawing;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FontAwesome.Sharp.Tests.WindowsForms
{
    internal static class TestExtensions
    {
        public static BitmapAssertions Should(this Bitmap bitmap)
        {
            return new(bitmap);
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
    }
}
