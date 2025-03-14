using Prism.Mvvm;
using Prism.Commands;
using CryptoViewer.Services;
using CryptoViewer.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoViewer.ViewModels
{
    public class SearchViewModel : BindableBase
    {
        private readonly CoinGeckoService _coinGeckoService;
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

        public SearchViewModel(CoinGeckoService coinGeckoService)
        {
            _coinGeckoService = coinGeckoService;
            SearchCommand = new DelegateCommand(async () => await ExecuteSearchAsync());
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
    }
}