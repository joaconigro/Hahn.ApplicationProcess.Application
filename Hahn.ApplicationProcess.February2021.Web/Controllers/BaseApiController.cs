using Hahn.Data.Pagination;
using Hahn.Data.Repositories;
using Hahn.Domain.Models;
using Hahn.Web.Dtos;
using Hahn.Web.Log;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hahn.Web.Controllers
{
    [Route("api/[controller]"), ApiController]
    public abstract class BaseApiController<TEntity, TDto> : Controller where TEntity : Entity where TDto : EntityDto
    {

        #region Constructor
        protected BaseApiController(IUnitOfWork unitOfWork, ILogManager log)
        {
            // Instantiate through DI
            UnitOfWork = unitOfWork;
            Log = log;

            // Instantiate a single JsonSerializerSettings object
            // that can be reused multiple times.
            JsonSettings = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        #endregion

        // GET: api/[controllerName]/all]
        [HttpGet("all")]
        public async Task<IActionResult> All([FromQuery] string orderBy, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(await OnGetAll(orderBy, pageNumber, pageSize));
        }

        protected virtual async Task<PagedList<TEntity>> OnGetAll(string orderBy, int pageNumber, int pageSize)
        {
            var entities = await UnitOfWork.Repository<TEntity>().GetEntitiesAsync(orderBy, pageNumber, pageSize);
            ProcessPagedListHeader(entities, typeof(TEntity).Name);
            return entities;
        }


        // GET: api/[controllerName]/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await OnGetting(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity.Adapt<TDto>());
        }

        protected virtual async Task<TEntity> OnGetting(int id)
        {
            return await UnitOfWork.Repository<TEntity>().GetByIdAsync(id);
        }







        // PUT: api/[controllerName]/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDto model)
        {
            await ValidateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await OnPuting(model);

            try
            {
                await UnitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                UnitOfWork.Rollback();
                var exists = await Exists(model);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(model);
        }

        protected virtual async Task OnPuting(TDto dto)
        {
            var newEntity = Adapt(dto);
            var oldEntity = await OnGetting(newEntity.Id);
            UnitOfWork.Repository<TEntity>().Update(oldEntity, newEntity);
        }

        // POST: api/[controllerName]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TDto model)
        {
            await ValidateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var entity = await OnPosting(model);

            await UnitOfWork.Repository<TEntity>().AddAsync(entity);

            try
            {
                await UnitOfWork.CommitAsync();
                return Created($"{GetBaseUrl()}/api/asset/{entity.Id}", entity.Adapt<TDto>());
            }
            catch (DbUpdateException)
            {
                UnitOfWork.Rollback();
                var exists = await Exists(model);
                if (exists)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual Task<TEntity> OnPosting(TDto dto)
        {
            return Task.FromResult(Adapt(dto));
        }

        // DELETE: api/[controllerName]/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await OnDeleting(id);
            if (entity == null)
            {
                return NotFound();
            }

            UnitOfWork.Repository<TEntity>().Delete(entity);
            await UnitOfWork.CommitAsync();

            return Ok(entity.Adapt<TDto>());
        }

        protected virtual async Task<TEntity> OnDeleting(int id)
        {
            return await OnGetting(id);
        }


        #region Helper methods
        protected async Task<bool> Exists(int id)
        {
            var entity = await OnGetting(id);
            return entity != null;
        }

        protected async Task<bool> Exists(TDto model)
        {
            return await Exists(model.Id);
        }

        protected void ProcessPagedListHeader<T>(PagedList<T> list, string key)
        {
            var metadata = new
            {
                key,
                list.Count,
                list.TotalCount,
                list.PageSize,
                list.CurrentPage,
                list.TotalPages,
                list.HasNext,
                list.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
        }

        protected TDto Adapt(TEntity entity)
        {
            return entity.Adapt<TDto>();
        }

        protected TEntity Adapt(TDto dto)
        {
            return dto.Adapt<TEntity>();
        }

        protected TEntity[] Adapt(IEnumerable<TDto> dtos)
        {
            return dtos.Adapt<TEntity[]>();
        }

        protected TDto[] Adapt(IEnumerable<TEntity> entities)
        {
            return entities.Adapt<TDto[]>();
        }

        protected abstract Task ValidateAsync(TDto dto);
       
        protected bool IsNull(TDto dto)
        {
            if (dto is null)
            {
                ModelState.AddModelError("NullBody", "Body cannot be null.");
            }
            return !ModelState.IsValid;
        }

        protected string GetBaseUrl()
        {
            return string.Format("{0}://{1}/", Request.Scheme, Request.Host.Value);
        }

        #endregion





        #region Properties
        protected ILogManager Log { get; private set; }
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected JsonSerializerOptions JsonSettings { get; private set; }

        #endregion
    }
}
