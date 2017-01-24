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
            if (value == null)
            {
                return null;
            }
            else if (value is InvFacility)
            {
                return (value as InvFacility).Facility_;
            }
            else if (value is InvEquipment)
            {
                return (value as InvEquipment).EquipmentID;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
