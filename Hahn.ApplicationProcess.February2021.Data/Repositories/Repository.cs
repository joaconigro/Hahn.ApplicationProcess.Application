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
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly DatabaseContext context;
        private readonly DbSet<T> entities;

        public Repository(DatabaseContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public virtual async Task<PagedList<T>> GetEntitiesAsync(string orderBy, int pageNumber = 1, int pageSize = 20)
        {
            return await entities.Sort(orderBy).ToPagedListAsync(pageNumber, pageSize);
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await entities.AddAsync(entity);
        }
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
        
        public virtual void Delete(T entity)
        {
            entities.Remove(entity);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}