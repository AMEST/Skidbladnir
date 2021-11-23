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
        public static IServiceCollection AddMongoDistributedCache(this IServiceCollection services, string connectionString)
        {
            BsonClassMap.RegisterClassMap(new CacheEntryMap());
            services.AddSingleton(r => new MongoDbContext(connectionString));
            return services.AddSingleton<IDistributedCache, MongoDbCache>();
        }
    }
}