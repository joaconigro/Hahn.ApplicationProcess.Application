using FluentValidation;
using Hahn.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Hahn.Domain.Validators
{
    public class AssetValidator : AbstractValidator<Asset>
    {
        public AssetValidator(Task<bool?> action)
        {
            RuleFor(a => a.Id)
                .NotNull()
                .WithErrorCode("AssetIdInvalid")
                .WithMessage("Id should not be null.");

            RuleFor(a => a.AssetName)
                .NotNull().WithErrorCode("InvalidAssetName")
                .NotEmpty().WithErrorCode("InvalidAssetName")
                .MinimumLength(5)
                .WithErrorCode("InvalidAssetName")
                .WithMessage("The name must have 5 characters length or greater.");

            RuleFor(a => (int)a.Department)
                .Custom((v, c) =>
                {
                    if (!Enum.IsDefined(typeof(Department), v))
                    {
                        c.AddFailure("InvalidDepartment", "Value is not a valid Department enum value.");
                    }
                });

            RuleFor(a => a.EmailAddressOfDepartment)
                .NotNull().WithErrorCode("InvalidEmailAddress")
                .NotEmpty().WithErrorCode("InvalidEmailAddress")
                .EmailAddress()
                .WithErrorCode("InvalidEmailAddress")
                .WithMessage("The value isn't a valid email address.");

            RuleFor(a => a.PurchaseDate)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithErrorCode("InvalidPurchaseDate")
                .WithMessage("The date can't be older than a year.");

            RuleFor(a => a.Broken)
                .NotNull()
                .WithErrorCode("AssetBrokenInvalid")
                .WithMessage("IsBroken property cannot be null.");

            RuleFor(a => a.CountryOfDepartment)
                .NotNull().WithErrorCode("InvalidCountryName")
                .NotEmpty().WithErrorCode("InvalidCountryName")
                .CustomAsync(async (v, c, t) =>
                {
                    var result = await action;
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
