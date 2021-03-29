using FluentValidation;

namespace Hahn.Domain.Validators
{
    public class AssetEmailAdressValidator : AbstractValidator<string>
    {
        public AssetEmailAdressValidator()
        {
            RuleFor(value => value)
                .EmailAddress()
                .WithErrorCode("AssetEMailAdressOfDepartmentInvalid")
                .WithMessage("The value isn't a valid email address.");
        }
    }
}
