using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Behaviors.Converters
{
   
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
  
            if (value is bool boolValue && boolValue)
            {
                return parameter?.ToString();
            }
            return null; 
        }
    }

    public class IsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
    
            if (value is bool boolValue && boolValue)
            {
                return parameter?.ToString();
            }
            return null;
        }
    }

    public class AllTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(v => v is bool b && b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
