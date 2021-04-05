using Hahn.Mobile.Services;
using Hahn.Mobile.Validators;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Hahn.Mobile.ViewModels
{
    public abstract class ValidatingFormViewModel<T> : BaseViewModel<T>
    {
        protected ValidatingFormViewModel(INavService nav, IHttpService http, IDialogService dialog) : base(nav, http, dialog)
        {
            AddValidationRules();
            SubmitCommand = new Command(async () => { await OnSubmit(); }, () => AreFieldsValid());
        }


        protected abstract void AddValidationRules();


        public ICommand ValidateStringPropertyCommand => new Command(async property =>
        {
            await ValidateProperty<string>(property);
        });

        public ICommand ValidateDatePropertyCommand => new Command(async property =>
        {
            await ValidateProperty<DateTime>(property);
        });

        public ICommand ValidateIntPropertyCommand => new Command(async property =>
        {
            await ValidateProperty<int>(property);
        });

        public ICommand ValidateDoublePropertyCommand => new Command(async property =>
        {
            await ValidateProperty<double>(property);
        });

        public ICommand ValidateBoolPropertyCommand => new Command(async property =>
        {
            await ValidateProperty<bool>(property);
        });

        private async Task ValidateProperty<TProperty>(object property)
        {
            var prop = (IValidatable<TProperty>)property;
            if (prop.IsAsync)
            {
                await prop.ValidateAsync();
            }
            else
            {
                prop.Validate();
            }
            SubmitCommand.ChangeCanExecute();
        }


        public Command SubmitCommand { get; }

        protected abstract Task OnSubmit();

        protected abstract bool AreFieldsValid();

    }
}
