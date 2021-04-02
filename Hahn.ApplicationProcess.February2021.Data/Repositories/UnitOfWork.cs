using Hahn.Data.Database;
using Hahn.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    /// <summary>
    /// Defines the <see cref="UnitOfWork" />.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Defines the _databaseContext.
        /// </summary>
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// Defines the _assetRepository.
        /// </summary>
        private IRepository<Asset> _assetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="databaseContext">The databaseContext<see cref="DatabaseContext"/>.</param>
        public UnitOfWork(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets the AssetRepository.
        /// </summary>
        public IRepository<Asset> AssetRepository
        {
            get { return _assetRepository ??= new Repository<Asset>(_databaseContext); }
        }

        /// <summary>
        /// Gets a Repository of type T.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="IRepository{T}"/>.</returns>
        public IRepository<T> Repository<T>() where T : Entity
        {
            return new Repository<T>(_databaseContext);
        }

        /// <summary>
        /// Save the changes in the persistence layer.
        /// </summary>
        public void Commit()
        {
            _databaseContext.SaveChanges();
        }

        /// <summary>
        /// Save the changes in the persistence layer.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CommitAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Undo the changes.
        /// </summary>
        public void Rollback()
        {
            _databaseContext.Dispose();
        }
    }
}