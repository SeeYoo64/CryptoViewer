using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace CryptoViewer
{
    public class LocalizedStrings
    {
        private static readonly ResourceManager _resourceManager;

        static LocalizedStrings()
        {
            _resourceManager = new ResourceManager("CryptoViewer.Resources.Strings", typeof(LocalizedStrings).Assembly);
        }

        public static string GetString(string key)
        {
            var culture = CultureInfo.CurrentUICulture;
            var result = _resourceManager.GetString(key, culture) ?? _resourceManager.GetString(key) ?? key; // Попробуем текущую культуру, затем дефолт
            System.Diagnostics.Debug.WriteLine($"GetString: Key={key}, Result={result}, Culture={culture.Name}");
            return result;
        }

        public static void SetCulture(CultureInfo culture)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            System.Diagnostics.Debug.WriteLine($"LocalizedStrings culture set to: {culture.Name}");
        }
    }
}