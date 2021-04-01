using FluentValidation;

namespace Hahn.Domain.Validators
{
    public class AssetEmailAdressValidator : AbstractValidator<string>
    {
        public AssetEmailAdressValidator()
        {
            RuleFor(value => value)
                .NotNull().WithErrorCode("InvalidEmailAddress")
                .NotEmpty().WithErrorCode("InvalidEmailAddress")
                .EmailAddress()
                .WithErrorCode("InvalidEmailAddress")
                .WithMessage("The value isn't a valid email address.");
        }
    }
}
