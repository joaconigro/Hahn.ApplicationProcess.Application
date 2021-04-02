using Hahn.Web.Dtos;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Hahn.Web.Swagger
{
    public class AssetsDtoExample : IExamplesProvider<IEnumerable<AssetDto>>
    {
        public IEnumerable<AssetDto> GetExamples()
        {
            return new List<AssetDto>
            {
                new AssetDto
                {
                    Id = 1,
                    AssetName = "Awesome Car",
                    Department = 2,
                    CountryOfDepartment = "Argentina",
                    EmailAddressOfDepartment = "my.email@someprovider.com",
                    PurchaseDate = new DateTime(2020, 5, 20, 8, 42, 0, DateTimeKind.Utc),
                    Broken = false
                }, 
                new AssetDto
                {
                    Id = 2,
                    AssetName = "Cool phone",
                    Department = 4,
                    CountryOfDepartment = "China",
                    EmailAddressOfDepartment = "my.other.email@someprovider.com",
                    PurchaseDate = new DateTime(2021, 3, 20, 15, 5, 0, DateTimeKind.Utc),
                    Broken = true
                },
                new AssetDto
                {
                    Id = 2,
                    AssetName = "Old Tv",
                    Department = 3,
                    CountryOfDepartment = "Italy",
                    EmailAddressOfDepartment = "my.other.email@someprovider.com",
                    PurchaseDate = new DateTime(2005, 9, 12, 20, 12, 2, DateTimeKind.Utc),
                    Broken = true
                }
            };
        }
    }
}
