using Hahn.Mobile.Properties;
using System.Threading.Tasks;

namespace Hahn.Mobile.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowAsync(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, Resources.Ok);
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, Resources.Yes, Resources.No);
        }
    }
}
