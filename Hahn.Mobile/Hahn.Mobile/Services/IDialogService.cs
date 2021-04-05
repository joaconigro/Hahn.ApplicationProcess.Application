using System.Threading.Tasks;

namespace Hahn.Mobile.Services
{
    public interface IDialogService
    {
        Task ShowAsync(string title, string message);
        Task<bool> ShowConfirmationAsync(string title, string message);
    }
}
