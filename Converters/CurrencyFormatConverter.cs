using System;
using System.Globalization;
using System.Windows.Data;

namespace CryptoViewer.Converters
{
    public class CurrencyFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is decimal amount && values[1] is string currency)
            {
                return string.Format(culture, "Result: {0:N2} {1}", amount, currency);
            }
            return "Result: N/A";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}