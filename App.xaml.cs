using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using CryptoViewer.ViewModels;
using CryptoViewer.Views;
using CryptoViewer.Services;

namespace CryptoViewer
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        // Here we will register services and views
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Registering ViewModel
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();

            // Registering service as singletone
            containerRegistry.RegisterSingleton<CoinGeckoService>();

            // Registering DetailsView
            containerRegistry.RegisterForNavigation<DetailsView, DetailsViewModel>();

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Navigation on main page on launch
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate("MainRegion", "MainView");

        }



    }
}