using Hahn.Data.Database;
using Hahn.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private IRepository<Asset> _assetRepository;
        
        public UnitOfWork(DatabaseContext databaseContext)
        { 
            _databaseContext = databaseContext; 
        }

        public IRepository<Asset> AssetRepository
        {
            get { return _assetRepository ??= new Repository<Asset>(_databaseContext); }
        }

        public IRepository<T> Repository<T>() where T : Entity
        {
            return new Repository<T>(_databaseContext);
        }

        public void Commit()
        {
            _databaseContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            _databaseContext.Dispose();
        }
    }
}
