using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Repository.MongoDB;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    public static class IocExtensions
    {
        /// <summary>
        /// Connecting the distributed cache module
        /// </summary>
        public static IMongoDbContextBuilder UseMongoDistributedCache(this IMongoDbContextBuilder builder,
            IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache, MongoDbCache>();
            return builder.AddEntity<CacheEntry, CacheEntryMap>();
        }

        /// <summary>
        /// Connecting the distributed cache module with using BaseMongoDbContext
        /// </summary>
        public static IServiceCollection UseMongoDistributedCache(this IServiceCollection services)
        {
            return services.UseMongoDistributedCache<BaseMongoDbContext>();
        }

        /// <summary>
        /// Connecting the distributed cache module with using Custom Mongo db context
        /// </summary>
        public static IServiceCollection UseMongoDistributedCache<TDbContext>(this IServiceCollection services)
            where TDbContext : BaseMongoDbContext
        {
            services.AddSingleton<IDistributedCache, MongoDbCache>();
            services.ConfigureMongoDbContext<TDbContext>(b => b.AddEntity<CacheEntry, CacheEntryMap>());
            return services;
        }
    }
}