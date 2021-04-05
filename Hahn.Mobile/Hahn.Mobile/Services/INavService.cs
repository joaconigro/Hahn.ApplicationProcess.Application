using Hahn.Mobile.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hahn.Mobile.Services
{
    public interface INavService
    {
        bool CanGoBack { get; }

        Task GoBack();

        Task NavigateTo<TVM>() where TVM : BaseViewModel;

        Task NavigateTo<TVM, TParameter>(TParameter parameter) where TVM : BaseViewModel;

        void RemoveLastView();

        void ClearBackStack();

        Task NavigateToUri(Uri uri);

        event PropertyChangedEventHandler CanGoBackChanged;

        event GoingBackEventHandler OnGoingBack;

        delegate void GoingBackEventHandler(object sender, object currentViewmodel);
    }
}
