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
        void Update(TEntity obj);

        /// <summary>
        /// Delete entity from storage
        /// </summary>
        void Delete(string id);

        /// <summary>
        /// Get entity from storage by id
        /// </summary>
        Task<TEntity> Get(string id);

        /// <summary>
        /// Get Quaryable with all entities
        /// </summary>
        IQueryable<TEntity> GetAll();
    }
}