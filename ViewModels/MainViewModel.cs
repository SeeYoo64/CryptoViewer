using CryptoViewer.Converters;
using CryptoViewer.Models;
using CryptoViewer.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace CryptoViewer.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly CoinGeckoService _coinGeckoService;
        private readonly IRegionManager _regionManager;
        private string _title = "CryptoViewer - Top Currencies";
        private ObservableCollection<Coin> _coins;
        private string _errorMessage;
        private string _selectedLanguage;
        private bool _isRefreshing;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<Coin> Coins
        {
            get => _coins;
            set => SetProperty(ref _coins, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    Debug.WriteLine($"SelectedLanguage changed to: {value}");
                    LocalizationManager.SetCulture(value);
                    RefreshLocalization();
                }
            }
        }

        public ObservableCollection<string> SupportedLanguages => new ObservableCollection<string>(LocalizationManager.SupportedCultures);

        public ICommand NavigateToDetailsCommand { get; }
        public ICommand NavigateToSearchCommand { get; }
        public ICommand NavigateToConverterCommand { get; }

        public MainViewModel(CoinGeckoService coinGeckoService, IRegionManager regionManager)
        {
            _coinGeckoService = coinGeckoService;
            _regionManager = regionManager;
            NavigateToDetailsCommand = new DelegateCommand<Coin>(NavigateToDetails);
            NavigateToSearchCommand = new DelegateCommand(NavigateToSearch);
            NavigateToConverterCommand = new DelegateCommand(NavigateToConverter);
            Coins = new ObservableCollection<Coin>();
            _isRefreshing = false;

            // Встановлюємо початкову мову з LocalizationManager, а не фіксовану "uk-UA"
            _selectedLanguage = LocalizationManager.CurrentCulture.Name; // Наприклад, "en-US" або "uk-UA"
            Debug.WriteLine($"Initial SelectedLanguage: {_selectedLanguage}");
            LoadCoinsAsync().ConfigureAwait(false);
        }

        private void NavigateToConverter()
        {
            Debug.WriteLine("Navigating to ConverterView.");
            _regionManager.RequestNavigate("MainRegion", "ConverterView");
        }

        private async Task LoadCoinsAsync()
        {
            try
            {
                ErrorMessage = null;
                var coins = await _coinGeckoService.GetTopCurrenciesAsync(10);
                if (coins == null || coins.Count == 0)
                {
                    ErrorMessage = "Failed to load currencies. Please check your internet connection or try again later.";
                }
                else
                {
                    Debug.WriteLine(coins.Count);
                    Coins = new ObservableCollection<Coin>(coins);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading currencies: {ex.Message}";
            }
        }

        private void NavigateToDetails(Coin coin)
        {
            if (coin != null)
            {
                var parameters = new NavigationParameters
                {
                    { "CoinId", coin.Id }
                };
                _regionManager.RequestNavigate("MainRegion", "DetailsView", parameters);
            }
        }

        private void NavigateToSearch()
        {
            _regionManager.RequestNavigate("MainRegion", "SearchView");
        }

        private void RefreshLocalization()
        {
            if (_isRefreshing) return;

            _isRefreshing = true;
            try
            {
                var region = _regionManager.Regions["MainRegion"];
                var currentView = region.ActiveViews.FirstOrDefault();
                if (currentView != null)
                {
                    var viewName = currentView.GetType().Name;
                    region.Remove(currentView);
                    var parameters = new NavigationParameters { { "refresh", Guid.NewGuid().ToString() } };
                    _regionManager.RequestNavigate("MainRegion", viewName, parameters);
                }
            }
            finally
            {
                _isRefreshing = false;
            }

            Debug.WriteLine($"Language changed to: {SelectedLanguage}, Culture: {CultureInfo.CurrentUICulture.Name}");
        }

    }
}