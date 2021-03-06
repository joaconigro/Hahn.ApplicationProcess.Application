using Hahn.Domain.Interfaces;
using System;

namespace Hahn.Domain.Models
{
    public class Asset : Entity
    {
        private DateTime purchaseDate;
        #region Properties
        public string AssetName { get; set; }
        public Department Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EmailAddressOfDepartment { get; set; }
        public DateTime PurchaseDate
        {
            get => purchaseDate;
            set
            {
                purchaseDate = value.ToUniversalTime();
            }
        }
        public bool Broken { get; set; }
        #endregion

        #region Methods
        public override void SetFrom(IEntity entity)
        {
            base.SetFrom(entity);
            var asset = (Asset)entity;
            AssetName = asset.AssetName;
            Department = asset.Department;
            CountryOfDepartment = asset.CountryOfDepartment;
            EmailAddressOfDepartment = asset.EmailAddressOfDepartment;
            PurchaseDate = asset.PurchaseDate;
            Broken = asset.Broken;
        }
        #endregion
    }

    public enum Department
    {
        HQ,
        Store1,
        Store2,
        Store3,
        MaintenanceStation
    }
}
