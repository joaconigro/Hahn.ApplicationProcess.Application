using Hahn.Mobile.Helpers;
using Hahn.Mobile.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Hahn.Mobile.Services
{
    public static class HttpHelper
    {
        public static string BaseAddress = Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:8080/api/" : "http://localhost:8080/api/";

        private static HttpClient client;

        public static HttpClient GetHttpClient()
        {
            if (client is null)
            {
#if DEBUG
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        if (cert.Issuer.Equals("CN=localhost"))
                            return true;
                        return errors == System.Net.Security.SslPolicyErrors.None;
                    }
                };

                client = new HttpClient(handler);
#else
                client = new HttpClient();
#endif
                client.BaseAddress = new Uri(BaseAddress);
            }

            return client;
        }

        public static string ParseQueryParams(Dictionary<string, string> parameters)
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

        public static HttpContent GetContent(object value)
        {
            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        public static async Task<T> Deserialize<T>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<T> SendRequestAsync<T>(string url, HttpMethod method, object requestData = null, IDictionary<string, string> headers = null)
        {
            var result = default(T);

            using (var request = new HttpRequestMessage(method, url))
            {
                // Add request data to request
                if (requestData != null)
                {
                    request.Content = GetContent(requestData);
                }

                // Add headers to request
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        request.Headers.Add(h.Key, h.Value);
                    }
                }

                // Get response
                try
                {
                    var client = GetHttpClient();
                    using var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                    var content = response.Content == null ? null : await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<T>(content);
                    }
                    else
                    {
                        HandleErrorResponse(response.StatusCode, response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is HttpException http)
                    {
                        throw;
                    }
                    throw new HttpException(ex);
                }

            }

            return result;
        }

        private static void HandleErrorResponse(HttpStatusCode code, string reasonPhrase)
        {
            throw code switch
            {
                HttpStatusCode.BadRequest => new HttpException((int)code, Resources.Error400Title, $"{Resources.Error400Message} {reasonPhrase}"),
                HttpStatusCode.Unauthorized => new HttpException((int)code, Resources.Error401Title, $"{Resources.Error401Message} {reasonPhrase}"),
                HttpStatusCode.Forbidden => new HttpException((int)code, Resources.Error403Title, $"{Resources.Error403Message} {reasonPhrase}"),
                HttpStatusCode.NotFound => new HttpException((int)code, Resources.Error404Title, $"{Resources.Error404Message} {reasonPhrase}"),
                HttpStatusCode.MethodNotAllowed => new HttpException((int)code, Resources.Error405Title, $"{Resources.Error405Message} {reasonPhrase}"),
                HttpStatusCode.InternalServerError => new HttpException((int)code, Resources.Error500Title, $"{Resources.Error500Message} {reasonPhrase}"),
                _ => new HttpException((int)code, Resources.UnknownError, $"{Resources.MoreInfo} {reasonPhrase}"),
            };
        }
    }
}
