using Hahn.Data.Repositories;
using Hahn.Domain.Models;
using Hahn.Web.Dtos;
using Hahn.Web.Log;
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
    }
}
