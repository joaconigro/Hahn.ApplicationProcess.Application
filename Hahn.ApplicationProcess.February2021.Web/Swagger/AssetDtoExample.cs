using Hahn.Web.Dtos;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Hahn.Web.Swagger
{
    public class AssetDtoExample : IMultipleExamplesProvider<AssetDto>
    {
        public IEnumerable<SwaggerExample<AssetDto>> GetExamples()
        {
            yield return SwaggerExample.Create("Post Test",
                new AssetDto
                {
                    AssetName = "A old door",
                    Department = 4,
                    CountryOfDepartment = "France",
                    EmailAddressOfDepartment = "my.email@someprovider.com",
                    PurchaseDate = new DateTime(2020, 5, 20, 8, 42, 0, DateTimeKind.Utc),
                    Broken = true
                });

            yield return SwaggerExample.Create("Put Test",
               new AssetDto
               {
                   Id = 2,
                   AssetName = "My Pro Phone",
                   Department = 2,
                   CountryOfDepartment = "Italy",
                   EmailAddressOfDepartment = "my.email@someprovider.com",
                   PurchaseDate = new DateTime(2020, 10, 20, 8, 42, 0, DateTimeKind.Utc),
                   Broken = false
               });
        }
    }
}
