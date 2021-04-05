using Hahn.Mobile.Dtos;
using Hahn.Mobile.Services;
using Hahn.Mobile.Validators;
using Hahn.Mobile.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hahn.Mobile.Properties;
using System.Linq;
using Hahn.Mobile.Helpers;
using Xamarin.Forms;

namespace Hahn.Mobile.ViewModels
{
    public class AssetDetailViewModel : ValidatingFormViewModel<AssetDto>
    {
        private AssetDto asset;
        private int assetId;
        private Command _deleteCommand;

        public ValidatableObject<string> AssetName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> CountryOfDepartment { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<int> Department { get; set; } = new ValidatableObject<int>();
        public ValidatableObject<DateTime> PurchaseDate { get; set; } = new ValidatableObject<DateTime>();
        public ValidatableObject<string> EmailAddressOfDepartment { get; set; } = new ValidatableObject<string>();
        public bool IsBroken { get; set; }

        public string[] Departments => Enumerable.Range(0, 5).Select(i => ResourceHelper.GetString($"Department{i}")).ToArray();

        public Command DeleteCommand => _deleteCommand ??= new Command(async () => await DeleteAsset());

        public bool IsEditing { get; private set; }


        public AssetDetailViewModel(INavService nav, IHttpService http) : base(nav, http)
        {
        }

        public override void Init(AssetDto parameter)
        {
            asset = parameter;
            if (asset != null)
            {
                assetId = asset.Id;
                IsEditing = true;
                Title = asset.AssetName;
                AssetName.Value = asset.AssetName;
                CountryOfDepartment.Value = asset.CountryOfDepartment;
                Department.Value = asset.Department;
                PurchaseDate.Value = asset.PurchaseDate;
                EmailAddressOfDepartment.Value = asset.EmailAddressOfDepartment;
                IsBroken = asset.Broken;
            }
            else
            {
                assetId =0;
                IsEditing = false;
                Title = Resources.AddAsset;
                Department.Value = 0;
                PurchaseDate.Value = DateTime.Now;
            }
            
        }

        protected override void AddValidationRules()
        {
            //AssetName Validation Rules
            AssetName.Validations.Add(new NotNullOrEmptyRule());
            AssetName.Validations.Add(new AsyncValidationRule<string>(Http, "asset/validateName", "assetName"));

            //CountryOfDepartment Validation Rules
            CountryOfDepartment.Validations.Add(new NotNullOrEmptyRule());
            CountryOfDepartment.Validations.Add(new AsyncValidationRule<string>(Http, "asset/validateCountry", "country"));

            //Last name Validation Rules
            Department.Validations.Add(new AsyncValidationRule<int>(Http, "asset/validateDepartment", "department"));

            //PurchaseDate Validation Rules
            PurchaseDate.Validations.Add(new AsyncValidationRule<DateTime>(Http, "asset/validateDate", "date"));

            //Email Validation Rules
            EmailAddressOfDepartment.Validations.Add(new NotNullOrEmptyRule());
            EmailAddressOfDepartment.Validations.Add(new AsyncValidationRule<string>(Http, "asset/validateEmail", "email"));
        }

        protected override bool AreFieldsValid()
        {
            bool isAssetNameValid = !string.IsNullOrEmpty(AssetName.Value) && AssetName.IsValid;
            bool isPurchaseDateValid = PurchaseDate.IsValid;
            bool isCountryValid = !string.IsNullOrEmpty(CountryOfDepartment.Value) && CountryOfDepartment.IsValid;
            bool isEmailValid = !string.IsNullOrEmpty(EmailAddressOfDepartment.Value) && EmailAddressOfDepartment.IsValid;
            bool isDepartmentValid = Department.IsValid;

            return isAssetNameValid && isPurchaseDateValid && isCountryValid
                   && isEmailValid && isDepartmentValid;
        }

        protected override async Task OnSubmit()
        {
            var asset = new AssetDto
            {
                AssetName = AssetName.Value,
                Broken = IsBroken,
                PurchaseDate = PurchaseDate.Value,
                CountryOfDepartment = CountryOfDepartment.Value,
                Department = Department.Value,
                EmailAddressOfDepartment = EmailAddressOfDepartment.Value,
                Id = assetId
            };

            AssetDto result;
            if (IsEditing)
            {
                result = await Http.UpdateItemAsync("asset", asset);
            }
            else
            {
                result = await Http.AddItemAsync("asset", asset);
            }

            if (result != null)
            {
                await NavService.GoBack();
            }
        }

        async Task DeleteAsset()
        {
            if (IsEditing)
            {
                var result = await Http.DeleteItemAsync<AssetDto>("asset", assetId.ToString());
                if (result != null)
                {
                    await NavService.GoBack();
                }
            }
        }
    }
}
