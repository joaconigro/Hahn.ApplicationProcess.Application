using Hahn.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.Data.Repositories
{
    /// <summary>
    /// Defines the <see cref="IUnitOfWork" />.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the AssetRepository.
        /// </summary>
        IRepository<Asset> AssetRepository { get; }

        /// <summary>
        /// Gets a Repository of type T.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="IRepository{T}"/>.</returns>
        IRepository<T> Repository<T>() where T : Entity;

        /// <summary>
        /// Save the changes in the persistence layer.
        /// </summary>
        void Commit();

        /// <summary>
        /// Save the changes in the persistence layer.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task CommitAsync();

        /// <summary>
        /// Undo the changes.
        /// </summary>
        void Rollback();
    }
}
