﻿using Prism.Mvvm;
using CryptoViewer.Models;
using CryptoViewer.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;


namespace CryptoViewer.ViewModels
{
    public class DetailsViewModel : BindableBase, INavigationAware
    {
        private readonly CoinGeckoService _coinGeckoService;
        private string _coinId;
        private Coin _coin;
        private ObservableCollection<Market> _markets;
        private string _errorMessage;

        public string CoinId
        {
            get => _coinId;
            set => SetProperty(ref _coinId, value);
        }

        public Coin Coin
        {
            get => _coin;
            set => SetProperty(ref _coin, value);
        }

        public ObservableCollection<Market> Markets
        {
            get => _markets;
            set => SetProperty(ref _markets, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand OpenTradeUrlCommand { get; }

        public DetailsViewModel(CoinGeckoService coinGeckoService)
        {
            _coinGeckoService = coinGeckoService;
            OpenTradeUrlCommand = new DelegateCommand<string>(OpenTradeUrl);
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("CoinId"))
            {
                CoinId = navigationContext.Parameters["CoinId"].ToString();
                // Log the received CoinId
                Debug.WriteLine($"DetailsViewModel: Received CoinId: {CoinId}");
                LoadCoinDetailsAsync().ConfigureAwait(false);
            }
            else
            {
                // Log if CoinId is not found
                Debug.WriteLine("DetailsViewModel: No CoinId provided in navigation parameters.");
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private async Task LoadCoinDetailsAsync()
        {
            try
            {
                ErrorMessage = null;
                if (!string.IsNullOrEmpty(CoinId))
                {
                    Coin = await _coinGeckoService.GetCoinDetailsAsync(CoinId);
                    var allMarkets = await _coinGeckoService.GetCoinMarketsAsync(CoinId);
                    if (Coin != null)
                    {
                        // Filtering markets
                        var filteredMarkets = allMarkets
                            .Where(m => m.Base != null && m.Base.Equals(Coin.Symbol, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        Markets = new ObservableCollection<Market>(filteredMarkets);
                    }
                    if (Coin == null)
                    {
                        ErrorMessage = "Failed to load coin details.";
                    }
                }
                else
                {
                    ErrorMessage = "No coin ID provided.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading details: {ex.Message}";
            }
        }


        private void OpenTradeUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error opening trade URL: {ex.Message}";
                }
            }
        }



    }
}