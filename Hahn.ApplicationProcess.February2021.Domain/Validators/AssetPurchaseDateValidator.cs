using FluentValidation;
using Hahn.Domain.Models;
using System;

namespace Hahn.Domain.Validators
{
    public class AssetPurchaseDateValidator : AbstractValidator<DateTime>
    {
        public AssetPurchaseDateValidator()
        {
            RuleFor(date => date)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithErrorCode("AssetPurchaseDateInvalid")
                .WithMessage("The date can't be older than a year.");
        }
    }
}
