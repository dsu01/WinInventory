using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Inventory.Business;

namespace Client.Converters
{
    [ValueConversion(typeof(InvFacility), typeof(string))]
    public class FacilityToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is InvFacility))
            {
                return null;
            }

            return (value as InvFacility).Facility_;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
