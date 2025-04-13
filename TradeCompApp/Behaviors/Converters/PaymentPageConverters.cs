using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Behaviors.Converters
{
     public class PaymentPageConverters
    {
       
    }
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Обратное преобразование: bool -> строка
            if (value is bool boolValue && boolValue)
            {
                return parameter?.ToString();
            }
            return null; // или string.Empty в зависимости от логики
        }
    }

    // IsNotNullConverter.cs
    public class IsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Обратное преобразование: bool -> строка
            if (value is bool boolValue && boolValue)
            {
                return parameter?.ToString();
            }
            return null; // или string.Empty в зависимости от логики
        }
    }

    // AllTrueConverter.cs
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
