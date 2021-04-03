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
                .WithErrorCode("InvalidAssetId");

            RuleFor(a => a.AssetName)
                .NotNull().WithErrorCode("InvalidAssetName")
                .NotEmpty().WithErrorCode("InvalidAssetName")
                .MinimumLength(5)
                .WithErrorCode("InvalidAssetName");

            RuleFor(a => (int)a.Department)
                .Custom((v, c) =>
                {
                    if (!Enum.IsDefined(typeof(Department), v))
                    {
                        c.AddFailure("InvalidDepartment");
                    }
                });

            RuleFor(a => a.EmailAddressOfDepartment)
                .NotNull().WithErrorCode("InvalidEmailAddress")
                .NotEmpty().WithErrorCode("InvalidEmailAddress")
                .EmailAddress()
                .WithErrorCode("InvalidEmailAddress");

            RuleFor(a => a.PurchaseDate)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithErrorCode("InvalidPurchaseDate");

            RuleFor(a => a.Broken)
                .NotNull()
                .WithErrorCode("InvalidBrokenValue");

            RuleFor(a => a.CountryOfDepartment)
                .NotNull().WithErrorCode("InvalidCountryName")
                .NotEmpty().WithErrorCode("InvalidCountryName")
                .CustomAsync(async (v, c, t) =>
                {
                    var result = await action;
                    if (result == false)
                    {
                        c.AddFailure("InvalidCountryName");
                    }
                    else if (!result.HasValue)
                    {
                        c.AddFailure("CountryNameCouldNotBeValidated");
                    }
                });
        }
    }
}
