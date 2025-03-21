using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace CryptoViewer.Views
{
    public partial class MainView
    {
        public MainView()
        {

            InitializeComponent();
            SwitchLanguage("en");
        }

        private void SwitchToEnglish(object sender, RoutedEventArgs e)
        {
            SwitchLanguage("en");
        }
        

        private void SwitchToUkranian(object sender, RoutedEventArgs e)
        {
            SwitchLanguage("ua");
        }
        private void SwitchLanguage(string culture)
        {
            // Determine the new language resource path
            string resourcePath = culture switch
            {
                "en" => "Resources/Resources.en.xaml",
                "ua" => "Resources/Resources.uk.xaml",
                _ => "Resources/Resources.en.xaml"
            };

            // Find the existing language dictionary in the merged dictionaries
            var langDictionary = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Resources/Resources"));

            // Remove old language dictionary if found
            if (langDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(langDictionary);
            }

            // Load and add the new language dictionary
            ResourceDictionary newLangDictionary = new ResourceDictionary { Source = new Uri(resourcePath, UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(newLangDictionary);

            // Set culture for formatting (optional)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture == "ua" ? "uk-UA" : "en-US");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked!");
        }
    }
}