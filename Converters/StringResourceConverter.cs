using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Globalization;

namespace CryptoViewer.Converters
{
    public class StringResourceConverter : IValueConverter, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StringResourceConverter()
        {
            // Subscribe to the CultureChanged event
            LocalizationManager.CultureChanged += OnCultureChanged;
        }

        private void OnCultureChanged(object sender, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine($"StringResourceConverter: Culture changed to {culture.Name}");
            Refresh();
        }

        private void Refresh()
        {
            // Notify bindings to update
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string key)
            {
                return LocalizedStrings.GetString(key) ?? key;
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}