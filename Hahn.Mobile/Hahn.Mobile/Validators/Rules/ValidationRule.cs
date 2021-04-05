using System.Threading.Tasks;

namespace Hahn.Mobile.Validators.Rules
{
    public abstract class ValidationRule<T> : IValidationRule<T>
    {
        public string Message { get; protected set; }
        public bool UseAsyncCheck { get; protected set; }
        public abstract bool Check(T value);

        public virtual async Task<string> CheckAsync(T value)
        {
            return await Task.FromResult(string.Empty);
        }

        protected ValidationRule(bool useAsync)
        {
            UseAsyncCheck = useAsync;
        }
    }
}
