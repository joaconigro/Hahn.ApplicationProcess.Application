using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Hahn.Domain.Validators
{
    public class AssetCountryValidator : AbstractValidator<string>
    {
        private readonly Task<bool?> checker;

        public AssetCountryValidator(Task<bool?> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            checker = action;

            RuleFor(value => value)
                .NotNull().WithErrorCode("InvalidCountryName")
                .NotEmpty().WithErrorCode("InvalidCountryName")
                .CustomAsync(async (v, c, t) =>
                {
                    var result = await checker;
                    if (result == false)
                    {
                        c.AddFailure("InvalidCountryName", "Value is not a valid country name.");
                    }
                    else if (!result.HasValue)
                    {
                        c.AddFailure("InvalidCountryName", "Country name couldn't be validated.");
                    }
                });
        }
    }
}
