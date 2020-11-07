using System;
using System.ComponentModel;
using System.Linq;

namespace FontAwesome.Sharp
{
    /// <summary>
    /// An icon enum converter to enhance the developer experience when choosing icons from a dropdown in designer.
    /// </summary>
    public class IconConverter : EnumConverter
    {
        public IconConverter(Type type)
            : base(type)
        {
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (Values != null)
                return Values;

            var values = Enum.GetValues(EnumType)
                .Cast<object>()
                .OrderBy(v => v.ToString())
                .ToArray();
            Values = new StandardValuesCollection(values);

            return Values;
        }
    }
}
