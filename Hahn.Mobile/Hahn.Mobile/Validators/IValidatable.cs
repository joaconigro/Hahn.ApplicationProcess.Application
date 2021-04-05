using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Hahn.Mobile.Validators
{
    public interface IValidatable<T> : INotifyPropertyChanged
    {
        List<IValidationRule<T>> Validations { get; }

        List<string> Errors { get; set; }

        bool Validate();

        Task<bool> ValidateAsync();

        bool IsValid { get; set; }
        bool IsAsync { get; }
    }
}
