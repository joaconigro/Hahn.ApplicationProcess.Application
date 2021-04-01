using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Hahn.Data.HTTPDataAccess;
using Hahn.Data.Repositories;
using Hahn.Domain.Models;
using Hahn.Domain.Validators;
using Hahn.Web.Dtos;
using Hahn.Web.Log;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.Web.Controllers
{
    public class AssetController : BaseApiController<Asset, AssetDto>
    {
        public AssetController(IUnitOfWork unitOfWork, ILogManager log) :
            base(unitOfWork, log)
        { }

        private ValidationResultDto GetValidationResultDto(ValidationResult result)
        {
            var dto = new ValidationResultDto
            {
                IsValid = result.IsValid,
                ErrorCode = result.Errors.FirstOrDefault()?.ErrorCode ?? result.Errors.FirstOrDefault()?.PropertyName,
                ErrorMessage = result.Errors.FirstOrDefault()?.ErrorMessage
            };
            return dto;
        }

        [HttpGet("validateId")]
        public async Task<IActionResult> IsValidAssetId([FromQuery] int id)
        {
            var validator = new AssetIdValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(id));
            return Ok(dto);
        }

        [HttpGet("validateName")]
        public async Task<IActionResult> IsValidAssetName([FromQuery] string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return Ok(new ValidationResultDto { IsValid = false, ErrorCode = "InvalidAssetName", ErrorMessage = "The name must have 5 characters length or greater."});
            }
            var validator = new AssetNameValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(assetName));
            return Ok(dto);
        }

        [HttpGet("validateDepartment")]
        public async Task<IActionResult> IsValidAssetDepartment([FromQuery] int department)
        {
            var validator = new AssetDepartmentValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(department));
            return Ok(dto);
        }

        [HttpGet("validateEmail")]
        public async Task<IActionResult> IsValidAssetEmailAddress([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Ok(new ValidationResultDto { IsValid = false, ErrorCode = "InvalidEmailAddress", ErrorMessage = "The value isn't a valid email address." });
            }
            var validator = new AssetEmailAdressValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(email));
            return Ok(dto);
        }

        [HttpGet("validateDate")]
        public async Task<IActionResult> IsValidAssetDate([FromQuery] DateTime date)
        {
            var validator = new AssetPurchaseDateValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(date));
            return Ok(dto);
        }

        [HttpGet("validateIsBroken")]
        public async Task<IActionResult> IsValidAssetBroken([FromQuery] bool broken)
        {
            var validator = new AssetBrokenValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(broken));
            return Ok(dto);
        }

        [HttpGet("validateCountry")]
        public async Task<IActionResult> IsValidAssetCountry([FromQuery] string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return Ok(new ValidationResultDto { IsValid = false, ErrorCode = "InvalidCountryName", ErrorMessage = "Value is not a valid country name." });
            }
            var url = $"https://restcountries.eu/rest/v2/name/{country}?fullText=true";
            var validator = new AssetCountryValidator(HTTPDataAccess.SendRequestAsync(url));
            var dto = GetValidationResultDto(await validator.ValidateAsync(country));
            return Ok(dto);
        }

        protected override async Task ValidateAsync(AssetDto dto)
        {
            if (IsNull(dto)) return;            
            var url = $"https://restcountries.eu/rest/v2/name/{dto.CountryOfDepartment}?fullText=true";
            var assetValidator = new AssetValidator(HTTPDataAccess.SendRequestAsync(url));
            var results = await assetValidator.ValidateAsync(new ValidationContext<Asset>(Adapt(dto)));
            results.AddToModelState(ModelState, null);
        }
    }
}
