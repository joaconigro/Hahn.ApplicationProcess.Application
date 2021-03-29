using FluentValidation;

namespace Hahn.Domain.Validators
{
    public class AssetNameValidator : AbstractValidator<string>
    {
        public AssetNameValidator()
        {
            RuleFor(name => name)
                .MinimumLength(5)
                .WithErrorCode("AssetNameInvalid")
                .WithMessage("The name must have 5 characters length or greater.");
        }
    }
}
