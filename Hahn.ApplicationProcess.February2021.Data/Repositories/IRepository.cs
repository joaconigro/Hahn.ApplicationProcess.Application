using Hahn.Data.Pagination;
using Hahn.Domain.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<PagedList<T>> GetEntitiesAsync(string orderBy, int pageNumber = 1, int pageSize = 20);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T oldEntity, T newEntity);
        void Delete(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}