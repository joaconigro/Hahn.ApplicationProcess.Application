using Hahn.Mobile.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.Mobile.Services
{
    public interface IHttpService
    {
        Task<T> AddItemAsync<T>(string url, T item);
        Task<T> DeleteItemAsync<T>(string url, string id);
        Task<T> GetItemAsync<T>(string url, string id);
        Task<T> GetItemAsync<T>(string url, Dictionary<string, string> parameters);
        Task<bool> ExistsAsync(string url, Dictionary<string, string> parameters);
        Task<ValidationResultDto> ValidateAsync(string url, Dictionary<string, string> parameters);
        Task<IEnumerable<T>> GetItemsAsync<T>(string url, bool forceRefresh = false);
        Task<IEnumerable<T>> GetItemsAsync<T>(string url, Dictionary<string, string> parameters, bool forceRefresh = false);
        Task<T> UpdateItemAsync<T>(string url, T item);
    }
}