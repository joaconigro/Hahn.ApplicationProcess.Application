using Hahn.Mobile.Helpers;
using Hahn.Mobile.Properties;
using Hahn.Mobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.Mobile.Validators.Rules
{
    public class AsyncValidationRule<T> : ValidationRule<T>
    {
        protected string queryKey;
        protected IHttpService http;
        protected string url;

        public AsyncValidationRule(IHttpService http, string url, string queryKey) : base(true)
        {
            this.http = http;
            this.url = url;
            this.queryKey = queryKey;
        }

        public override bool Check(T value)
        {
            return true;
        }

        public override async Task<string> CheckAsync(T value)
        {
            if (value != null)
            {
                var parameters = new Dictionary<string, string>
                {
                    {queryKey, value.ToString() }
                };

                try
                {
                    var result = await http.ValidateAsync(url, parameters);
                    if (!result.IsValid)
                    {
                        return ResourceHelper.GetString(result.ErrorCode);
                    }
                }
                catch
                {
                    return Resources.NotValidated;
                }

            }

            return string.Empty;
        }
    }
}
