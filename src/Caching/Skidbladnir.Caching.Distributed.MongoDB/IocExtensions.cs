using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    public static class IocExtensions
    {
        /// <summary>
        ///     Add MongoDB Distributed Cache
        /// </summary>
        public static IServiceCollection AddMongoDistributedCache(this IServiceCollection services,
            string connectionString)
        {
            return services.AddMongoDistributedCache(new DistributedCacheMongoModuleConfiguration()
                {ConnectionString = connectionString});
        }

        /// <summary>
        ///     Add MongoDB Distributed Cache
        /// </summary>
        public static IServiceCollection AddMongoDistributedCache(this IServiceCollection services,
            DistributedCacheMongoModuleConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap(new CacheEntryMap());
            return services
                .AddSingleton(configuration)
                .AddSingleton<MongoDbContext>()
                .AddSingleton<IDistributedCache, MongoDbCache>();
        }
    }
}