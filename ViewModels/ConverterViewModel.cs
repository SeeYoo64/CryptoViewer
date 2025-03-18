using Prism.Mvvm;
using Prism.Commands;
using CryptoViewer.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace CryptoViewer.ViewModels
{
    public class ConverterViewModel : BindableBase, INavigationAware
    {
        private readonly CoinGeckoService _coinGeckoService;
        private readonly IRegionManager _regionManager;
        private string _amount;
        private string _selectedFromCurrency;
        private string _selectedToCurrency;
        private decimal _convertedAmount;
        private string _errorMessage;
        private ObservableCollection<string> _coinIds;
        private ObservableCollection<string> _vsCurrencies;

        public string Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public ObservableCollection<string> CoinIds
        {
            get => _coinIds;
            set => SetProperty(ref _coinIds, value);
        }

        public ObservableCollection<string> VsCurrencies
        {
            get => _vsCurrencies;
            set => SetProperty(ref _vsCurrencies, value);
        }

        public string SelectedFromCurrency
        {
            get => _selectedFromCurrency;
            set => SetProperty(ref _selectedFromCurrency, value);
        }

        public string SelectedToCurrency
        {
            get => _selectedToCurrency;
            set => SetProperty(ref _selectedToCurrency, value);
        }

        public decimal ConvertedAmount
        {
            get => _convertedAmount;
            set => SetProperty(ref _convertedAmount, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand ConvertCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public ConverterViewModel(CoinGeckoService coinGeckoService, IRegionManager regionManager)
        {
            _coinGeckoService = coinGeckoService;
            _regionManager = regionManager;
            ConvertCommand = new DelegateCommand(async () => await ExecuteConvertAsync());
            NavigateBackCommand = new DelegateCommand(NavigateBack);

            // Initialize collections dynamically
            LoadCurrenciesAsync().ConfigureAwait(false);
        }

        private async Task LoadCurrenciesAsync()
        {
            try
            {
                var fiatCurrencies = await _coinGeckoService.GetSupportedCurrenciesAsync();
                var coinIds = await _coinGeckoService.GetCoinIdsAsync();
                Debug.WriteLine($"Fiat currencies count: {fiatCurrencies?.Count ?? 0}");
                Debug.WriteLine($"Coin IDs count: {coinIds?.Count ?? 0}");

                if (fiatCurrencies != null && coinIds != null)
                {
                    CoinIds = new ObservableCollection<string>(coinIds);
                    VsCurrencies = new ObservableCollection<string>(fiatCurrencies);
                    // Set default selections
                    SelectedFromCurrency = CoinIds.FirstOrDefault(c => c == "bitcoin") ?? CoinIds.FirstOrDefault();
                    SelectedToCurrency = VsCurrencies.FirstOrDefault(c => c == "usd") ?? VsCurrencies.FirstOrDefault();
                    Debug.WriteLine($"Coin IDs loaded: {CoinIds.Count}, Vs Currencies loaded: {VsCurrencies.Count}");
                }
                else
                {
                    ErrorMessage = "Failed to load supported currencies.";
                    CoinIds = new ObservableCollection<string> { "bitcoin" }; // Fallback
                    VsCurrencies = new ObservableCollection<string> { "usd" }; // Fallback
                    SelectedFromCurrency = "bitcoin";
                    SelectedToCurrency = "usd";
                    Debug.WriteLine("Fallback currencies applied.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading currencies: {ex.Message}";
                CoinIds = new ObservableCollection<string> { "bitcoin" }; // Fallback
                VsCurrencies = new ObservableCollection<string> { "usd" }; // Fallback
                SelectedFromCurrency = "bitcoin";
                SelectedToCurrency = "usd";
                Debug.WriteLine($"Error in LoadCurrenciesAsync: {ex.Message}");
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private async Task ExecuteConvertAsync()
        {
            try
            {
                ErrorMessage = null;
                ConvertedAmount = 0;

                if (string.IsNullOrEmpty(Amount) || !decimal.TryParse(Amount, out decimal amountValue) || amountValue <= 0)
                {
                    ErrorMessage = "Please enter a valid amount.";
                    return;
                }

                if (string.IsNullOrEmpty(SelectedFromCurrency) || string.IsNullOrEmpty(SelectedToCurrency))
                {
                    ErrorMessage = "Please select both currencies.";
                    return;
                }

                var toCurrencies = new List<string> { SelectedToCurrency };
                var rates = await _coinGeckoService.GetExchangeRatesAsync(SelectedFromCurrency, toCurrencies);
                Debug.WriteLine($"Exchange rates received: {JsonConvert.SerializeObject(rates)}"); // Log all rates

                if (rates.ContainsKey(SelectedToCurrency) && rates[SelectedToCurrency] > 0)
                {
                    ConvertedAmount = amountValue * rates[SelectedToCurrency];
                    Debug.WriteLine($"Converted {amountValue} {SelectedFromCurrency} to {ConvertedAmount} {SelectedToCurrency}");
                }
                else
                {
                    ErrorMessage = "Exchange rate not available.";
                    Debug.WriteLine($"No valid rate for {SelectedFromCurrency} to {SelectedToCurrency}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error converting currency: {ex.Message}";
                Debug.WriteLine($"Error in ExecuteConvertAsync: {ex.Message}");
            }
        }

        private void NavigateBack()
        {
            Debug.WriteLine("Navigating back to MainView from ConverterView.");
            _regionManager.RequestNavigate("MainRegion", "MainView");
        }
    }
}