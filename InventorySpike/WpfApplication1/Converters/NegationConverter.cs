using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Client.Converters
{
    [ValueConversion(typeof(object), typeof(object))]
    public class NegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    return !(Boolean)value;
                case TypeCode.Decimal:
                    return -(Decimal)value;
                case TypeCode.Double:
                    return -(Double)value;
                case TypeCode.Int16:
                    return -(Int16)value;
                case TypeCode.Int32:
                    return -(Int32)value;
                case TypeCode.Int64:
                    return -(Int64)value;
                case TypeCode.SByte:
                    return -(SByte)value;
                case TypeCode.Single:
                    return -(Single)value;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    throw new ArgumentException(String.Format("Illegal attempt to negate an unsigned number {0}", value.GetType()), "value");
                default:
                    throw new ArgumentException(String.Format("Illegal attempt to negate a non numeric type {0}", value.GetType()), "value");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
