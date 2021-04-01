using FluentValidation;

namespace Hahn.Domain.Validators
{
    public class AssetIdValidator : AbstractValidator<int>
    {
        public AssetIdValidator()
        {
            RuleFor(id => id)
                .NotNull()
                .WithErrorCode("AssetIdInvalid")
                .WithMessage("Id should not be null.");
        }
    }
}
