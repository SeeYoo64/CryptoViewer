using Prism.Mvvm;

namespace CryptoViewer.ViewModels
{
    public class DetailsViewModel : BindableBase, INavigationAware
    {
        private string _coinId;

        public string CoinId
        {
            get => _coinId;
            set => SetProperty(ref _coinId, value);
        }

        public DetailsViewModel()
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("CoinId"))
            {
                CoinId = navigationContext.Parameters["CoinId"].ToString();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}