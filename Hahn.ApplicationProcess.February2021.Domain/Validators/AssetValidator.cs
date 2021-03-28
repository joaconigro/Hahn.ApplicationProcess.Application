using FluentValidation;
using Hahn.Domain.Models;
using System;

namespace Hahn.Domain.Validators
{
    public class AssetValidator : AbstractValidator<Asset>
    {
        public AssetValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("AssetIdInvalid");
            RuleFor(x => x.AssetName).MinimumLength(5).WithMessage("AssetNameInvalid");
            RuleFor(x => x.Department).IsInEnum().WithMessage("AssetDepartmentInvalid");
            RuleFor(x => x.PurchaseDate).GreaterThan(DateTime.Now.AddYears(-1)).WithMessage("AssetPurchaseDateInvalid");
            RuleFor(x => x.EMailAdressOfDepartment).EmailAddress().WithMessage("AssetEMailAdressOfDepartmentInvalid");
            RuleFor(x => x.Broken).NotNull().WithMessage("AssetBrokenInvalid");
        }
    }
}
