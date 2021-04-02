using Hahn.Data.Database;
using Hahn.Data.Pagination;
using Hahn.Data.Sorting;
using Hahn.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    /// <summary>
    /// Defines the <see cref="Repository{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class Repository<T> : IRepository<T> where T : Entity
    {
        /// <summary>
        /// Defines the context.
        /// </summary>
        protected readonly DatabaseContext context;

        /// <summary>
        /// Defines the entities.
        /// </summary>
        private readonly DbSet<T> entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="DatabaseContext"/>.</param>
        public Repository(DatabaseContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        /// <summary>
        /// Returns a <see cref="PagedList{IEntity}"/>.
        /// </summary>
        /// <param name="orderBy">The orderBy use a property name, and optionally a code like ASC or DESC, separated by semicolon. Example: Id;Desc.</param>
        /// <param name="pageNumber">The page number to return a paged list.</param>
        /// <param name="pageSize">The page size to return a paged list.</param>
        public virtual async Task<PagedList<T>> GetEntitiesAsync(string orderBy, int pageNumber = 1, int pageSize = 20)
        {
            return await entities.Sort(orderBy).ToPagedListAsync(pageNumber, pageSize);
        }

        /// <summary>
        /// Returns a <see cref="PagedList{IEntity}"/>.
        /// </summary>
        /// <typeparam name="TResult">.</typeparam>
        /// <param name="orderBy">The orderBy use a property name, and optionally a code like ASC or DESC, separated by semicolon. Example: Id;Desc.</param>
        /// <param name="selector">The selector <see cref="Func{T, TResult}"/> that projects the entities.</param>
        /// <param name="pageNumber">The page number to return a paged list.</param>
        /// <param name="pageSize">The page size to return a paged list.</param>
        public virtual async Task<PagedList<TResult>> GetEntitiesAsync<TResult>(string orderBy, Func<T, TResult> selector, int pageNumber = 1, int pageSize = 20)
        {
            return await entities.Sort(orderBy).ToPagedListAsync(pageNumber, pageSize, selector);
        }

        /// <summary>
        /// Returns the <see cref="T"/> entity that has that Id. Return null if it's not found.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Add an entity to the Database.
        /// </summary>
        /// <param name="entity">The entity <see cref="T"/> to be added.</param>
        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await entities.AddAsync(entity);
        }

        /// <summary>
        /// Updates an old entity with the values of the new one.
        /// </summary>
        /// <param name="oldEntity">The old entity.</param>
        /// <param name="newEntity">The new entity<see cref="T"/>.</param>
        public virtual void Update(T oldEntity, T newEntity)
        {
            if (oldEntity == null)
            {
                throw new ArgumentNullException(nameof(oldEntity));
            }
            else if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }

            oldEntity.SetFrom(newEntity);
        }

        /// <summary>
        /// Removes an entity from the Database.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        public virtual void Delete(T entity)
        {
            entities.Remove(entity);
        }

        /// <summary>
        /// Returns the <see cref="T"/> entity that match the predicate. Return null if it's not found.
        /// </summary>
        /// <param name="predicate">The predicate <see cref="Expression{Func{T, bool}}"/>.</param>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}
