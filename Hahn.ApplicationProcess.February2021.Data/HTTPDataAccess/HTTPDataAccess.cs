using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.Data.HTTPDataAccess
{
    /// <summary>
    /// Defines the <see cref="HttpDataAccess"/> helper class.
    /// </summary>
    public static class HttpDataAccess
    {
        /// <summary>
        /// Parse a <see cref="IDictionary<string, string>"/> to create a Url string.
        /// </summary>
        /// <param name="parameters">The parameters <see cref="IDictionary{string, string}"/>.</param>
        internal static string ParseQueryParams(IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                var keys = parameters.Keys.ToArray();
                var values = parameters.Values.ToArray();
                var query = string.Empty;
                for (int i = 0; i < parameters.Count; i++)
                {
                    string conector = i == 0 ? "?" : "&";
                    query += $"{conector}{keys[i]}={values[i]}";
                }
                return query;
            }
            return string.Empty;
        }

        /// <summary>
        /// Send a request to the Url, with the parameters and headers passed.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="parameters">The parameters <see cref="IDictionary{string, string}"/>.</param>
        /// <param name="headers">The headers <see cref="IDictionary{string, string}"/>.</param>
        /// <returns>The true if the response has a success code (2xx), false if returns a 404, and null otherwise.</returns>
        public static async Task<bool?> SendRequestAsync(string url, IDictionary<string, string> parameters = null, IDictionary<string, string> headers = null)
        {
            //Add paramters to the url
            var fullUrl = $"{url}{ParseQueryParams(parameters)}";

            try
            {
                using var client = new HttpClient();

                // Add headers to the http client
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    }
                }

                // Get the response
                using var response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
