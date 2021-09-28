using System.Linq;
using System.Threading.Tasks;

namespace Skidbladnir.Repository.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Add new entity to storage
        /// </summary>
        Task Create(TEntity obj);

        /// <summary>
        /// Update entity in storage
        /// </summary>
        Task Update(TEntity obj);

        /// <summary>
        /// Delete entity from storage
        /// </summary>
        Task Delete(TEntity obj);

        /// <summary>
        /// Get Quaryable with all entities
        /// </summary>
        IQueryable<TEntity> GetAll();
    }
}