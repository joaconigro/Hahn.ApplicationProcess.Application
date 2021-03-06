using System;

namespace Hahn.Mobile.Dtos
{
    public class AssetDto : EntityDto
    {
        #region Properties
        public string AssetName { get; set; }
        public int Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EmailAddressOfDepartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool Broken { get; set; }
        #endregion
    }
}
