using Hahn.Mobile.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.Mobile.Services
{
    public class HttpService : IHttpService
    {
        public async Task<T> AddItemAsync<T>(string url, T item)
        {
            return await HttpHelper.SendRequestAsync<T>(url, HttpMethod.Post, item);
        }

        public async Task<T> DeleteItemAsync<T>(string url, string id)
        {
            return await HttpHelper.SendRequestAsync<T>($"{url}/{id}", HttpMethod.Delete);
        }

        public async Task<T> GetItemAsync<T>(string url, string id)
        {
            return await HttpHelper.SendRequestAsync<T>($"{url}/{id}", HttpMethod.Get);
        }

        public async Task<T> GetItemAsync<T>(string url, Dictionary<string, string> parameters)
        {
            var completeUrl = $"{url}{HttpHelper.ParseQueryParams(parameters)}";
            return await HttpHelper.SendRequestAsync<T>(completeUrl, HttpMethod.Get);
        }

        public async Task<bool> ExistsAsync(string url, Dictionary<string, string> parameters)
        {
            var query = $"{url}{HttpHelper.ParseQueryParams(parameters)}";
            return await HttpHelper.SendRequestAsync<bool>(query, HttpMethod.Get);
        }

        public async Task<ValidationResultDto> ValidateAsync(string url, Dictionary<string, string> parameters)
        {
            var query = $"{url}{HttpHelper.ParseQueryParams(parameters)}";
            return await HttpHelper.SendRequestAsync<ValidationResultDto>(query, HttpMethod.Get);
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string url, bool forceRefresh = false)
        {
            return await HttpHelper.SendRequestAsync<T[]>(url, HttpMethod.Get);
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string url, Dictionary<string, string> parameters, bool forceRefresh = false)
        {
            var completeUrl = $"{url}{HttpHelper.ParseQueryParams(parameters)}";
            return await HttpHelper.SendRequestAsync<T[]>(completeUrl, HttpMethod.Get);
        }

        public async Task<T> UpdateItemAsync<T>(string url, T item)
        {
            return await HttpHelper.SendRequestAsync<T>(url, HttpMethod.Put, item);
        }
    }
}
