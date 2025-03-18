using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using CryptoViewer.ViewModels;
using CryptoViewer.Views;
using CryptoViewer.Services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CryptoViewer
{
    public partial class App : PrismApplication
    {
        private IConfiguration _configuration;
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        public App()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
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

            // Registering Search System
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();

            // Registering configuration file
            containerRegistry.RegisterInstance<IConfiguration>(_configuration);

            // Registering ConverterView
            containerRegistry.RegisterForNavigation<ConverterView, ConverterViewModel>();
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