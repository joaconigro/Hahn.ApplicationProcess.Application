using FluentValidation;
using Hahn.Domain.Models;
using System;

namespace Hahn.Domain.Validators
{
    public class AssetDepartmentValidator : AbstractValidator<int>
    {
        public AssetDepartmentValidator()
        {
            RuleFor(value => value)
                .Custom((v, c) =>
                {
                    if (!Enum.IsDefined(typeof(Department), v))
                    {
                        c.AddFailure("InvalidDepartment");
                    }
                });
        }
    }
}
