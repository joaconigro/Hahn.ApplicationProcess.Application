using FluentValidation;
using FluentValidation.Results;
using Hahn.Data.HTTPDataAccess;
using Hahn.Data.Repositories;
using Hahn.Domain.Models;
using Hahn.Domain.Validators;
using Hahn.Web.Dtos;
using Hahn.Web.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.Web.Controllers
{
    /// <summary>
    /// Defines the <see cref="AssetController" />.
    /// </summary>
    public class AssetController : BaseApiController<Asset, AssetDto>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork <see cref="IUnitOfWork"/>.</param>
        /// <param name="log">The log manager <see cref="ILogManager"/>.</param>
        /// <param name="localizer">The localizer <see cref="IStringLocalizer"/>.</param>
        public AssetController(IUnitOfWork unitOfWork, ILogManager log, IStringLocalizer localizer) :
            base(unitOfWork, log, localizer)
        {
        }
        #endregion

        #region Validators
        /// <summary>
        /// Converts a ValidationResult to ValidationResultDto.
        /// </summary>
        /// <param name="result">The <see cref="ValidationResult"/>.</param>
        /// <returns>The <see cref="ValidationResultDto"/>.</returns>
        internal ValidationResultDto GetValidationResultDto(ValidationResult result)
        {
            var dto = new ValidationResultDto
            {
                IsValid = result.IsValid,
                ErrorCode = result.Errors.FirstOrDefault()?.ErrorCode ?? result.Errors.FirstOrDefault()?.ErrorMessage
            };
            dto.ErrorMessage = !dto.IsValid ? Localizer[dto.ErrorCode] : null;
            return dto;
        }

        /// <summary>
        /// Validates if Id property is not null.
        /// </summary>
        /// <param name="id">The id.</param>
        [HttpGet("validateId")]
        public async Task<IActionResult> IsValidAssetId([FromQuery] int id)
        {
            var validator = new AssetIdValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(id));
            return Ok(dto);
        }

        /// <summary>
        /// Validates if AssetName property is not null and at least has 5 characters.
        /// </summary>
        /// <param name="assetName">The asset name.</param>
        [HttpGet("validateName")]
        public async Task<IActionResult> IsValidAssetName([FromQuery] string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return Ok(new ValidationResultDto
                {
                    IsValid = false,
                    ErrorCode = "InvalidAssetName",
                    ErrorMessage = Localizer["InvalidAssetName"]
                });
            }
            var validator = new AssetNameValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(assetName));
            return Ok(dto);
        }

        /// <summary>
        /// Validates if the Department property is a valid enum value (0 - 4).
        /// </summary>
        /// <param name="department">The department value.</param>
        [HttpGet("validateDepartment")]
        public async Task<IActionResult> IsValidAssetDepartment([FromQuery] int department)
        {
            var validator = new AssetDepartmentValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(department));
            return Ok(dto);
        }

        /// <summary>
        /// Check if the email address is a valid email.
        /// </summary>
        /// <param name="email">The email.</param>
        [HttpGet("validateEmail")]
        public async Task<IActionResult> IsValidAssetEmailAddress([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Ok(new ValidationResultDto
                {
                    IsValid = false,
                    ErrorCode = "InvalidEmailAddress",
                    ErrorMessage = Localizer["InvalidEmailAddress"]
                });
            }
            var validator = new AssetEmailAdressValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(email));
            return Ok(dto);
        }

        /// <summary>
        /// Check if the date isn't older than a year.
        /// </summary>
        /// <param name="date" example="2019-05-20T08:42:00Z">The date.</param>
        [HttpGet("validateDate")]
        public async Task<IActionResult> IsValidAssetDate([FromQuery] DateTime date)
        {
            var validator = new AssetPurchaseDateValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(date));
            return Ok(dto);
        }

        /// <summary>
        /// Validates if Broken property is not null.
        /// </summary>
        /// <param name="broken">The broken.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet("validateIsBroken")]
        public async Task<IActionResult> IsValidAssetBroken([FromQuery] bool broken)
        {
            var validator = new AssetBrokenValidator();
            var dto = GetValidationResultDto(await validator.ValidateAsync(broken));
            return Ok(dto);
        }

        /// <summary>
        /// Validates a country name using the restcountries.eu API.
        /// </summary>
        /// <param name="country">The country.</param>
        [HttpGet("validateCountry")]
        public async Task<IActionResult> IsValidAssetCountry([FromQuery] string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return Ok(new ValidationResultDto
                {
                    IsValid = false,
                    ErrorCode = "InvalidCountryName",
                    ErrorMessage = Localizer["InvalidCountryName"]
                });
            }
            var url = $"https://restcountries.eu/rest/v2/name/{country}?fullText=true";
            var validator = new AssetCountryValidator(HttpDataAccess.SendRequestAsync(url));
            var dto = GetValidationResultDto(await validator.ValidateAsync(country));
            return Ok(dto);
        }

        /// <summary>
        /// Validates all properties of properties of an AssetDto object.
        /// </summary>
        /// <param name="dto">The <see cref="AssetDto"/>.</param>
        protected override async Task ValidateAsync(AssetDto dto)
        {
            if (IsNull(dto)) return;
            var url = $"https://restcountries.eu/rest/v2/name/{dto.CountryOfDepartment}?fullText=true";
            var assetValidator = new AssetValidator(HttpDataAccess.SendRequestAsync(url));
            var results = await assetValidator.ValidateAsync(new ValidationContext<Asset>(Adapt(dto)));
            foreach (var e in results.Errors)
            {
                ModelState.AddModelError(e.PropertyName, Localizer[e.ErrorCode ?? e.ErrorMessage]);
            }
        }
        #endregion
    }
}
