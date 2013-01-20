using System;
using System.ComponentModel;
using System.Globalization;

namespace StasisEditor.Views.Controls
{
    public class XNAColorConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
                return new XNAColor((string)value);
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is XNAColor)
                return (value as XNAColor).ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
