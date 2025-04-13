using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Repository.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Add new entity to storage
        /// </summary>
        Task Create(TEntity obj, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Add new entities to storage
        /// </summary>
        Task CreateAll(IEnumerable<TEntity> objs, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update entity in storage
        /// </summary>
        Task Update(TEntity obj, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete entity from storage
        /// </summary>
        Task Delete(TEntity obj, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete many entities from storage
        /// </summary>
        Task DeleteAll(IEnumerable<TEntity> objs, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete many entities by expression from storage
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAll(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get Quaryable with all entities
        /// </summary>
        IQueryable<TEntity> GetAll();
    }
}