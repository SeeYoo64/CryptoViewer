using Prism.Mvvm;
using Prism.Commands;
using CryptoViewer.Services;
using CryptoViewer.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace CryptoViewer.ViewModels
{
    public class SearchViewModel : BindableBase
    {
        private readonly CoinGeckoService _coinGeckoService;
        private readonly IRegionManager _regionManager;
        private string _searchQuery;
        private ObservableCollection<SearchResult> _searchResults;
        private string _errorMessage;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public ObservableCollection<SearchResult> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand NavigateToDetailsCommand { get; }

        public SearchViewModel(CoinGeckoService coinGeckoService, IRegionManager regionManager)
        {
            _coinGeckoService = coinGeckoService;
            _regionManager = regionManager;
            SearchCommand = new DelegateCommand(async () => await ExecuteSearchAsync());
            NavigateToDetailsCommand = new DelegateCommand<SearchResult>(NavigateToDetails);
            SearchResults = new ObservableCollection<SearchResult>();
        }

        private async Task ExecuteSearchAsync()
        {
            try
            {
                ErrorMessage = null;
                if (!string.IsNullOrEmpty(SearchQuery))
                {
                    var results = await _coinGeckoService.SearchCoinsAsync(SearchQuery);
                    SearchResults.Clear();
                    foreach (var result in results)
                    {
                        SearchResults.Add(result);
                    }
                    // Log the number of search results
                    Debug.WriteLine($"Search results loaded: {SearchResults.Count} items.");
                }
                else
                {
                    SearchResults.Clear();
                    ErrorMessage = "Please enter a search query.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error searching coins: {ex.Message}";
            }
        }

        private void NavigateToDetails(SearchResult searchResult)
        {
            // Log whether the method is called
            Debug.WriteLine("NavigateToDetails method called.");

            if (searchResult != null)
            {
                // Log the CoinId being navigated to
                Debug.WriteLine($"Navigating to details for coin ID: {searchResult.Id}");
                var parameters = new NavigationParameters
        {
            { "CoinId", searchResult.Id }
        };
                _regionManager.RequestNavigate("MainRegion", "DetailsView", parameters);
            }
            else
            {
                // Log if searchResult is null
                Debug.WriteLine("SearchResult is null, cannot navigate.");
            }
        }
    }
}