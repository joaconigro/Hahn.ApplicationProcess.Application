using FluentValidation;

namespace Hahn.Domain.Validators
{
    public class AssetNameValidator : AbstractValidator<string>
    {
        public AssetNameValidator()
        {
            RuleFor(name => name)
                .NotNull().WithErrorCode("InvalidAssetName")
                .NotEmpty().WithErrorCode("InvalidAssetName")
                .MinimumLength(5)
                .WithErrorCode("InvalidAssetName");
        }
    }
}
