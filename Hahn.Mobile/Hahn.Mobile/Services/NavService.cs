using Hahn.Mobile.Bootstrap;
using Hahn.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Hahn.Mobile.Services.INavService;

namespace Hahn.Mobile.Services
{
    public class NavService : INavService
    {
        readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();

        public event PropertyChangedEventHandler CanGoBackChanged;

        public event GoingBackEventHandler OnGoingBack;

        public INavigation XamarinFormsNav { get; set; }

        public bool CanGoBack => XamarinFormsNav.NavigationStack?.Any() == true;

        public async Task GoBack()
        {
            if (CanGoBack)
            {
                await XamarinFormsNav.PopAsync(true);
                OnCanGoBackChanged();
                OnGoingBackChanged();
            }
        }

        public async Task NavigateTo<TVM>() where TVM : BaseViewModel
        {
            var vm = GetViewModel<object>(typeof(TVM), null);
            await NavigateToView(vm);
        }

        public async Task NavigateTo<TVM, TParameter>(TParameter parameter) where TVM : BaseViewModel
        {
            var vm = GetViewModel(typeof(TVM), parameter);
            await NavigateToView(vm);
        }

        object GetViewModel<TParameter>(Type viewModelType, TParameter parameter)
        {
            var vm = AppContainer.Resolve(viewModelType);

            if (vm is BaseViewModel<TParameter> viewmodelParam)
            {
                viewmodelParam.Init(parameter);
            }
            else if (vm is BaseViewModel viewModel)
            {
                viewModel.Init();
            }

            return vm;
        }

        public void RemoveLastView()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
            {
                return;
            }

            var lastView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 2];

            XamarinFormsNav.RemovePage(lastView);
        }

        public void ClearBackStack()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
            {
                return;
            }

            for (var i = 0; i < XamarinFormsNav.NavigationStack.Count - 1; i++)
            {
                XamarinFormsNav.RemovePage(XamarinFormsNav.NavigationStack[i]);
            }
        }

        public async Task NavigateToUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentException("Invalid URI");
            }

            await Launcher.OpenAsync(uri);
        }

        async Task NavigateToView(object vm)
        {
            if (!_map.TryGetValue(vm.GetType(), out Type viewType))
            {
                throw new ArgumentException("No view found in view mapping for " + vm.GetType().FullName + ".");
            }

            // Use reflection to get the View's constructor and create an instance of the View
            var constructor = viewType.GetTypeInfo()
                                      .DeclaredConstructors
                                      .FirstOrDefault(dc => !dc.GetParameters().Any());
            var view = constructor.Invoke(null) as Page;

            view.BindingContext = vm;
            await XamarinFormsNav.PushAsync(view, true);
        }

        public void RegisterViewMapping(Type viewModel, Type view)
        {
            _map.Add(viewModel, view);
        }

        void OnCanGoBackChanged() => CanGoBackChanged?.Invoke(this, new PropertyChangedEventArgs("CanGoBack"));

        void OnGoingBackChanged()
        {
            var vm = XamarinFormsNav.NavigationStack.Last().BindingContext;
            OnGoingBack?.Invoke(this, vm);
        }
    }
}
