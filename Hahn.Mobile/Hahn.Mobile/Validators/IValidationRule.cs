using System.Threading.Tasks;

namespace Hahn.Mobile.Validators
{
    public interface IValidationRule<T>
    {
        public string Message { get; }
        public bool UseAsyncCheck { get; }
        bool Check(T value);

        Task<string> CheckAsync(T value);
    }
}
