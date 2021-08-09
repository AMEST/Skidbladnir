using Microsoft.EntityFrameworkCore;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public interface IContextBuilder<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Добавление сущности
        /// </summary>
        IContextBuilder<TContext> AddEntity<TEntity>()
            where TEntity : class, IHasId<int>;

        /// <summary>
        /// Добавление сущности вместе с описанием
        /// </summary>
        IContextBuilder<TContext> AddEntity<TEntity, TConfiguration>()
            where TConfiguration : IEntityTypeConfiguration<TEntity>, new()
            where TEntity : class, IHasId<int>;
    }
}