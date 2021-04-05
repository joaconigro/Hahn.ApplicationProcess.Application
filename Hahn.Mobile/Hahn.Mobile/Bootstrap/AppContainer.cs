using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Hahn.Mobile.Services;
using Hahn.Mobile.ViewModels;
using System;
using Hahn.Mobile.Views;

namespace Hahn.Mobile.Bootstrap
{
    public static class AppContainer
    {
        private static IContainer container;

        public static void Init()
        {
            //Create the builder
            var builder = new ContainerBuilder();

            //Register the services
            builder.RegisterType<NavService>().As<INavService>().SingleInstance();
            builder.RegisterType<HttpService>().As<IHttpService>().SingleInstance();

            //Register the view models
            builder.RegisterType<AssetDetailViewModel>();
            builder.RegisterType<AssetsViewModel>();

            //Build the container
            container = builder.Build();
            AutofacServiceLocator autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);

            // Register view mappings
            var navService = Resolve<INavService>() as NavService;
            navService.RegisterViewMapping(typeof(AssetDetailViewModel), typeof(AssetDetailPage));
            navService.RegisterViewMapping(typeof(AssetsViewModel), typeof(AssetsPage));
        }

        public static T Resolve<T>() => container.Resolve<T>();

        public static object Resolve(Type type) => container.Resolve(type);
    }

}
