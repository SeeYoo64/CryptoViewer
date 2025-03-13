using CryptoViewer.Models;
using CryptoViewer.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public ICommand NavigateToDetailsCommand { get; }

        public MainViewModel(CoinGeckoService coinGeckoService, IRegionManager regionManager)
        {
            _coinGeckoService = coinGeckoService;
            _regionManager = regionManager;
            NavigateToDetailsCommand = new DelegateCommand<Coin>(NavigateToDetails);
            LoadCoinsAsync().ConfigureAwait(false);
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
    }
}