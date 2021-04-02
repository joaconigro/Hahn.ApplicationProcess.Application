using Hahn.Domain.Models;
using System;
using System.Collections.Generic;

namespace Hahn.Data.Database
{
    public static class DbSeeder
    {
        #region Public Methods
        public static void Seed(DatabaseContext dbContext)
        {
            //Create some fake assets.
            var assets = new List<Asset>
            {
                new Asset
                {
                    AssetName = "Awesome Car",
                    Department  = Department.HQ,
                    CountryOfDepartment = "Argentina",
                    EmailAddressOfDepartment = "my.email@someprovider.com",
                    PurchaseDate = DateTime.Now.AddDays(-30)
                },
                new Asset
                {
                    AssetName = "My Pro Phone",
                    Department  = Department.Store3,
                    CountryOfDepartment = "Japan",
                    EmailAddressOfDepartment = "some.email@otherprovider.com",
                    PurchaseDate = DateTime.Now.AddDays(-180),
                    Broken = true
                }
            };

            //Add those assets to the db.
            dbContext.Assets.AddRange(assets);

            //Save db changes.
            dbContext.SaveChanges();
        }
        #endregion
    }
}

