using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.MongoDB
{
    public interface IMongoDbContextBuilder
    {
        /// <summary>
        /// Register entity, entity configuration for with using this db context
        /// </summary>
        IMongoDbContextBuilder AddEntity<TEntity, TEntityConfiguration>()
            where TEntity: class, IHasId
            where TEntityConfiguration : EntityMapClass<TEntity>, new();

        /// <summary>
        /// Congigure connection string
        /// </summary>
        IMongoDbContextBuilder UseConnectionString(string connectionString);
    }
}