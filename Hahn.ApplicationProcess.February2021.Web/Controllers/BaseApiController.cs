using Hahn.Data.Pagination;
using Hahn.Data.Repositories;
using Hahn.Domain.Models;
using Hahn.Web.Dtos;
using Hahn.Web.Log;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hahn.Web.Controllers
{
    /// <summary>
    /// Defines the <see cref="BaseApiController{TEntity, TDto}" />.
    /// </summary>
    /// <typeparam name="TEntity">The Entity type from Domain.</typeparam>
    /// <typeparam name="TDto">The DataTable Object (DTO) that avoids to expose sensitive properties from domain.</typeparam>
    [Route("api/[controller]"), ApiController]
    public abstract class BaseApiController<TEntity, TDto> : Controller where TEntity : Entity where TDto : EntityDto
    {
        #region Properties
        /// <summary>
        /// The LogManager property.
        /// </summary>
        protected ILogManager Log { get; private set; }

        /// <summary>
        /// The UnitOfWork property.
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController{TEntity, TDto}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork <see cref="IUnitOfWork"/>.</param>
        /// <param name="log">The log manager <see cref="ILogManager"/>.</param>
        protected BaseApiController(IUnitOfWork unitOfWork, ILogManager log)
        {
            // Instantiate through DI
            UnitOfWork = unitOfWork;
            Log = log;
        }
        #endregion

        #region Main REST methods
        /// <summary>
        /// Returns a list of DTO.
        /// </summary>
        /// <param name="orderBy" example="Id;Desc">The orderBy use a property name, and optionally a code like ASC or DESC, separated by semicolon. Example: Id;Desc.</param>
        /// <param name="pageNumber" example="1">The page number to return a paged list.</param>
        /// <param name="pageSize" example="10">The page size to return a paged list.</param>
        /// <returns>Returns a <see cref="PagedList{TDto}"/>.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> All([FromQuery] string orderBy, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(await OnGetAll(orderBy, pageNumber, pageSize));
        }

        /// <summary>
        /// A virtual method that gets all the DTO.
        /// </summary>
        /// <param name="orderBy">The orderBy use a property name, and optionally a code like ASC or DESC, separated by semicolon. Example: Id;Desc.</param>
        /// <param name="pageNumber">The page number to return a paged list.</param>
        /// <param name="pageSize">The page size to return a paged list.</param>
        /// <returns>The <see cref="PagedList{TDto}"/>.</returns>
        protected virtual async Task<PagedList<TDto>> OnGetAll(string orderBy, int pageNumber, int pageSize)
        {
            var entities = await UnitOfWork.Repository<TEntity>().GetEntitiesAsync(orderBy, e => Adapt(e), pageNumber, pageSize);
            ProcessPagedListHeader(entities, typeof(TEntity).Name);
            return entities;
        }

        /// <summary>
        /// The Get method.
        /// </summary>
        /// <param name="id" example="1">The Id of the entity that is requesting.</param>
        /// <returns>The <see cref="TDto"/>.</returns>
        /// <response code="200">Entity found.</response>
        /// <response code="404">Entity not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await OnGetting(id);
            if (entity == null)
            {
                Log.LogInfo($"Trying to get an entity of type {typeof(TEntity).Name} with Id {id} was not found.");
                return NotFound();
            }
            return Ok(Adapt(entity));
        }

        /// <summary>
        /// A virtual method that gets the entity.
        /// </summary>
        /// <param name="id">The Id of the entity that is requesting.</param>
        /// <returns>The <see cref="TDto"/>.</returns>
        protected virtual async Task<TEntity> OnGetting(int id)
        {
            return await UnitOfWork.Repository<TEntity>().GetByIdAsync(id);
        }

        /// <summary>
        /// The Put method.
        /// </summary>
        /// <param name="dto">The object <see cref="TDto"/> that represents the updated object.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="200">Entity updated.</response>
        /// <response code="400">Entity not found.</response>
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDto dto)
        {
            //Validates the Dto.
            await ValidateAsync(dto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check if exists an entity with same Id.
            var exists = await Exists(dto);
            if (!exists)
            {
                Log.LogInfo($"Trying to update an entity of type {typeof(TEntity).Name} with Id {dto.Id} was not found.");
                return BadRequest($"Trying to update an entity of type {typeof(TEntity).Name} with Id {dto.Id} was not found.");
            }

            //Upadates the old entity.
            await OnPuting(dto);

            try
            {
                //Save the changes.
                await UnitOfWork.CommitAsync();
            }
            catch
            {
                //Rollback the changes if failed.
                UnitOfWork.Rollback();
                throw;
            }

            return Ok(dto);
        }

        /// <summary>
        /// A virtual method that update the old entity.
        /// </summary>
        /// <param name="dto">The object <see cref="TDto"/> that represents the updated object.</param>
        protected virtual async Task OnPuting(TDto dto)
        {
            var newEntity = Adapt(dto);
            var oldEntity = await OnGetting(newEntity.Id);
            UnitOfWork.Repository<TEntity>().Update(oldEntity, newEntity);
        }

        /// <summary>
        /// The Post method.
        /// </summary>
        /// <param name="dto">The new <see cref="TDto"/> that will be saved.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="201">Entity created. The new entity Url is passed in the 'location' header.</response>
        /// <response code="400">Found another entity with same Id.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TDto dto)
        {
            //Validates the Dto.
            await ValidateAsync(dto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check if exists an entity with same Id.
            var exists = await Exists(dto);
            if (exists)
            {
                Log.LogInfo($"Trying to create an entity of type {typeof(TEntity).Name} with Id {dto.Id}, but already exist another with same Id.");
                return BadRequest($"Trying to create an entity of type {typeof(TEntity).Name} with Id {dto.Id}, but already exist another with same Id.");
            }

            //Create the entity from the Dto.
            var entity = await OnPosting(dto);

            //Add the entity to the repository
            await UnitOfWork.Repository<TEntity>().AddAsync(entity);

            try
            {
                //Save the changes.
                await UnitOfWork.CommitAsync();
                return Created($"{GetBaseUrl()}api/asset/{entity.Id}", entity.Adapt<TDto>());
            }
            catch
            {
                //Rollback the changes if failed.
                UnitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// A virtual method that adapt the Dto to an Entity.
        /// </summary>
        /// <param name="dto">The dto<see cref="TDto"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        protected virtual Task<TEntity> OnPosting(TDto dto)
        {
            return Task.FromResult(Adapt(dto));
        }

        /// <summary>
        /// The Delete method.
        /// </summary>
        /// <param name="id" example="1">The Id of the entity that is deleting.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        /// <response code="200">Entity deleted.</response>
        /// <response code="404">Entity not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //Gets the entity.
            var entity = await OnDeleting(id);
            if (entity == null)
            {
                Log.LogInfo($"Trying to delete an entity of type {typeof(TEntity).Name} with Id {id} was not found.");
                return NotFound();
            }

            //Deletes the entity and save the changes.
            UnitOfWork.Repository<TEntity>().Delete(entity);
            await UnitOfWork.CommitAsync();

            return Ok(Adapt(entity));
        }

        /// <summary>
        /// A virtual method that gets the entity to be deleted.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        protected virtual async Task<TEntity> OnDeleting(int id)
        {
            return await OnGetting(id);
        }
        #endregion


        #region Helper methods
        /// <summary>
        /// The Exists.
        /// </summary>
        /// <param name="id">The Id of the entity that is deleting.</param>
        protected async Task<bool> Exists(int id)
        {
            var entity = await OnGetting(id);
            return entity != null;
        }

        /// <summary>
        /// Check if exist an <see cref="TEntity"/> with the same Id as the <see cref="TDto"/>.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>Returns true if the Entity exists, false otherwise.</returns>
        protected async Task<bool> Exists(TDto dto)
        {
            return await Exists(dto.Id);
        }

        /// <summary>
        /// Process a <see cref="PagedList{T}"/> and add info to the X-Pagination Header.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="list">The <see cref="PagedList{T}"/>.</param>
        /// <param name="key">The key to identify that allows to handle several PagedList info from the headers.</param>
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

        /// <summary>
        /// Convert an <see cref="TEntity"/> to a <see cref="TDto"/>.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        /// <returns>The <see cref="TDto"/>.</returns>
        protected TDto Adapt(TEntity entity)
        {
            return entity.Adapt<TDto>();
        }

        /// <summary>
        /// Convert an <see cref="TDto"/> to a <see cref="TEntity"/>.
        /// </summary>
        /// <param name="dto">The <see cref="TDto"/>.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        protected TEntity Adapt(TDto dto)
        {
            return dto.Adapt<TEntity>();
        }

        /// <summary>
        /// Convert an <see cref="IEnumerable{TDto}"/> to a <see cref="TEntity"/> array.
        /// </summary>
        /// <param name="dtos">The <see cref="IEnumerable{TDto}"/>.</param>
        protected TEntity[] Adapt(IEnumerable<TDto> dtos)
        {
            return dtos.Adapt<TEntity[]>();
        }

        /// <summary>
        /// Convert an <see cref="IEnumerable{TEntity}"/> to a <see cref="TDto"/> array.
        /// </summary>
        /// <param name="entities">The <see cref="IEnumerable{TEntity}"/>.</param>
        protected TDto[] Adapt(IEnumerable<TEntity> entities)
        {
            return entities.Adapt<TDto[]>();
        }

        /// <summary>
        /// An abstract method to validates the Dto before Put or Post.
        /// </summary>
        /// <param name="dto">The dto<see cref="TDto"/>.</param>
        protected abstract Task ValidateAsync(TDto dto);

        /// <summary>
        /// Check if the Dto if null.
        /// </summary>
        /// <param name="dto">The dto<see cref="TDto"/>.</param>
        /// <returns>Returs true if its null.</returns>
        protected bool IsNull(TDto dto)
        {
            if (dto is null)
            {
                ModelState.AddModelError("NullBody", "Body cannot be null.");
            }
            return !ModelState.IsValid;
        }

        /// <summary>
        /// Get the BaseUrl from the request.
        /// </summary>
        protected string GetBaseUrl()
        {
            return string.Format("{0}://{1}/", Request.Scheme, Request.Host.Value);
        }
        #endregion
    }
}
