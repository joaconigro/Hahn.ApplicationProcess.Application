using Hahn.Data.Pagination;
using Hahn.Domain.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    /// <summary>
    /// Defines the <see cref="IRepository{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Returns a <see cref="PagedList{IEntity}"/>.
        /// </summary>
        /// <param name="orderBy">The orderBy use a property name, and optionally a code like ASC or DESC, separated by semicolon. Example: Id;Desc.</param>
        /// <param name="pageNumber">The page number to return a paged list.</param>
        /// <param name="pageSize">The page size to return a paged list.</param>
        Task<PagedList<T>> GetEntitiesAsync(string orderBy, int pageNumber = 1, int pageSize = 20);

        /// <summary>
        /// Returns a <see cref="PagedList{IEntity}"/>.
        /// </summary>
        /// <typeparam name="TResult">.</typeparam>
        /// <param name="orderBy">The orderBy use a property name, and optionally a code like ASC or DESC, separated by semicolon. Example: Id;Desc.</param>
        /// <param name="selector">The selector <see cref="Func{T, TResult}"/> that projects the entities.</param>
        /// <param name="pageNumber">The page number to return a paged list.</param>
        /// <param name="pageSize">The page size to return a paged list.</param>
        Task<PagedList<TResult>> GetEntitiesAsync<TResult>(string orderBy, Func<T, TResult> selector, int pageNumber = 1, int pageSize = 20);

        /// <summary>
        /// Returns the <see cref="T"/> entity that has that Id. Return null if it's not found.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Add an entity to the Database.
        /// </summary>
        /// <param name="entity">The entity <see cref="T"/> to be added.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an old entity with the values of the new one.
        /// </summary>
        /// <param name="oldEntity">The old entity.</param>
        /// <param name="newEntity">The new entity<see cref="T"/>.</param>
        void Update(T oldEntity, T newEntity);

        /// <summary>
        /// Removes an entity from the Database.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        void Delete(T entity);

        /// <summary>
        /// Returns the <see cref="T"/> entity that match the predicate. Return null if it's not found.
        /// </summary>
        /// <param name="predicate">The predicate <see cref="Expression{Func{T, bool}}"/>.</param>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
