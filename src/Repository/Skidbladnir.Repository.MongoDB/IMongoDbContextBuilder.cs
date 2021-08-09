using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.MongoDB
{
    public interface IMongoDbContextBuilder
    {
        /// <summary>
        /// Register entity with default entity configuration
        /// </summary>
        IMongoDbContextBuilder AddEntity<TEntity>()
            where TEntity : class, IHasId<string>;

        /// <summary>
        /// Register entity, entity configuration
        /// </summary>
        IMongoDbContextBuilder AddEntity<TEntity, TEntityConfiguration>()
            where TEntity: class, IHasId<string>
            where TEntityConfiguration : EntityMapClass<TEntity>, new();

        /// <summary>
        /// Configure connection string
        /// </summary>
        IMongoDbContextBuilder UseConnectionString(string connectionString);

        /// <summary>
        /// Configure retry count
        /// </summary>
        IMongoDbContextBuilder UseRetry(int retryCount);
    }
}