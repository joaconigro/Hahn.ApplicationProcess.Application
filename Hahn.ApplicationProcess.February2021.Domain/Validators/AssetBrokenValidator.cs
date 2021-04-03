using FluentValidation;

namespace Hahn.Domain.Validators
{
    public class AssetBrokenValidator : AbstractValidator<bool>
    {
        public AssetBrokenValidator()
        {
            RuleFor(broken => broken)
                .NotNull()
                .WithErrorCode("InvalidBrokenValue");
        }
    }
}
