using Hahn.Mobile.Bootstrap;
using Hahn.Mobile.Services;
using Hahn.Mobile.ViewModels;
using Hahn.Mobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hahn.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppContainer.Init();
            var mainPage = new NavigationPage(new AssetsPage());
            var vm = AppContainer.Resolve<AssetsViewModel>();
            mainPage.BindingContext = vm;
            var navService = AppContainer.Resolve<INavService>() as NavService;
            navService.XamarinFormsNav = mainPage.Navigation;
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
