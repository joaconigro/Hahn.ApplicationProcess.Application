using System;

namespace Hahn.Web.Dtos
{
    public class AssetDto : EntityDto
    {
        #region Properties
        public string AssetName { get; set; }
        public int Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EmailAdressOfDepartment { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool Broken { get; set; }
        #endregion
    }
}
