using FluentValidation;
using System;

namespace Hahn.Domain.Validators
{
    public class AssetPurchaseDateValidator : AbstractValidator<DateTime>
    {
        public AssetPurchaseDateValidator()
        {
            RuleFor(date => date)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithErrorCode("InvalidPurchaseDate");
        }
    }
}
