using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hahn.Mobile.Validators
{
    public abstract class Validatable<T> : IValidatable<T>
    {
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();
        public List<string> Errors { get; set; } = new List<string>();
        public bool CleanOnChange { get; set; } = true;
        public bool IsValid { get; set; } = true;
        public abstract bool Validate();
        public abstract Task<bool> ValidateAsync();

        public bool IsAsync => Validations.Any(v => v.UseAsyncCheck);


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
