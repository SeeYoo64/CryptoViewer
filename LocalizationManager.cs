using System;
using System.Globalization;
using System.Collections.Generic;

namespace CryptoViewer
{
    public static class LocalizationManager
    {
        // Define the CultureChanged event
        public static event EventHandler<CultureInfo> CultureChanged;

        private static string _currentLanguage;

        private static CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

        public static CultureInfo CurrentCulture
        {
            get { return _currentCulture; }
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    // Optional: Add logic to notify the app of culture changes
                }
            }
        }
        public static IReadOnlyList<string> SupportedCultures { get; } = new List<string>
        {
            "en-US",
            "uk-UA"
        }.AsReadOnly();

        public static string CurrentLanguage
        {
            get => _currentLanguage;
            private set => _currentLanguage = value;
        }

        static LocalizationManager()
        {
            _currentLanguage = "uk-UA"; // Default language
            SetCulture(_currentLanguage);
        }

        public static void SetCulture(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName) || !SupportedCultures.Contains(cultureName))
            {
                cultureName = "en-US"; // Fallback culture
            }

            _currentLanguage = cultureName;
            _currentCulture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = _currentCulture;
            CultureInfo.CurrentUICulture = _currentCulture;
            LocalizedStrings.SetCulture(_currentCulture);

            // Trigger the CultureChanged event
            CultureChanged?.Invoke(null, _currentCulture);
            System.Diagnostics.Debug.WriteLine($"LocalizationManager: Culture set to {cultureName}");
        }
    }
}