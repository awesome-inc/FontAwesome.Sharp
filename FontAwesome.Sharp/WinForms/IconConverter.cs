using System;
using System.ComponentModel;
using System.Linq;

namespace FontAwesome.Sharp
{
    public class IconConverter : EnumConverter
    {
        public IconConverter(Type type)
            : base(type)
        {
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (Values == null)
            {
                var values = Enum.GetValues(EnumType)
                    .Cast<object>()
                    .OrderBy(v => v.ToString())
                    .ToArray();
                Values = new StandardValuesCollection(values);
            }

            return Values;
        }
    }
}
