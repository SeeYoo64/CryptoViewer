using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CryptoViewer.Converters
{
    public class LocalizedFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
                return string.Empty;

            string valueToFormat = values[0]?.ToString(); // The value (e.g., Coin.Name)
            string resourceKey = values[1] as string;      // The resource key (e.g., "DetailsTitleFormat")

            if (string.IsNullOrEmpty(resourceKey))
                return valueToFormat ?? string.Empty;

            // Resolve the localized string from the application's resources
            string formatString = Application.Current.TryFindResource(resourceKey) as string;

            if (string.IsNullOrEmpty(formatString) || !formatString.Contains("{0}"))
                return valueToFormat ?? string.Empty;

            try
            {
                return string.Format(formatString, valueToFormat);
            }
            catch (FormatException ex)
            {
                return $"[Error: Invalid Format] {valueToFormat}";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}