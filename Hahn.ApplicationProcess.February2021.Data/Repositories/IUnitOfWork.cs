using Hahn.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Asset> AssetRepository { get; }
        IRepository<T> Repository<T>() where T : Entity;
        void Commit();
        Task CommitAsync();
        void Rollback();
    }
}