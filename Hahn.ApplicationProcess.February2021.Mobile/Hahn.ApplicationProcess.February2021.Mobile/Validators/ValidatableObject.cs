using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.Mobile.Validators
{
    public class ValidatableObject<T> : Validatable<T>
    {
        T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                if (CleanOnChange)
                    IsValid = true;
            }
        }

        public override bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.UseAsyncCheck && !v.Check(Value))
                .Select(v => v.Message);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            OnPropertyChanged(nameof(Errors));
            OnPropertyChanged(nameof(IsValid));

            return IsValid;
        }
        public override string ToString()
        {
            return $"{Value}, is valid = {IsValid}";
        }

        public override async Task<bool> ValidateAsync()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations
                .Where(v => !v.UseAsyncCheck && !v.Check(Value))
                .Select(v => v.Message);

            if (!errors.Any())
            {
                var tasks = Validations.Where(v => v.UseAsyncCheck)
                .Select(v => v.CheckAsync(Value)).ToList();

                var asyncResults = await Task.WhenAll(tasks);
                var results = asyncResults.Where(r => !string.IsNullOrEmpty(r));
                errors = results;
            }

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            OnPropertyChanged(nameof(Errors));
            OnPropertyChanged(nameof(IsValid));

            return IsValid;
        }
    }
}