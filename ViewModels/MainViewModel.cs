using Prism.Mvvm;
using CryptoViewer.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CryptoViewer.Models;

namespace CryptoViewer.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly CoinGeckoService _coinGeckoService;
        private string _title = "CryptoViewer Main Page";
        private ObservableCollection<Coin> _coins;
        private string _errorMessage;

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

        public MainViewModel(CoinGeckoService coinGeckoService)
        {
            _coinGeckoService = coinGeckoService;
            LoadCoinsAsync().ConfigureAwait(false);
        }

        private async Task LoadCoinsAsync()
        {
            try
            {
                ErrorMessage = null; // Reset Error msg
                var coins = await _coinGeckoService.GetTopCurrenciesAsync(10);
                if (coins == null || coins.Count == 0)
                {
                    ErrorMessage = "Failed to load currencies. Please check your internet connection or try again later.";
                }
                else
                {
                    Coins = new ObservableCollection<Coin>(coins);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading currencies: {ex.Message}";
            }
        }
    }
}