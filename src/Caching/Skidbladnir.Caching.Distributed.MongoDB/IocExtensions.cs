using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Repository.MongoDB;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    public static class IocExtensions
    {
        public static IMongoDbContextBuilder UseMongoDistributedCache(this IMongoDbContextBuilder builder,
            IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache, MongoDbCache>();
            return builder.AddEntity<CacheEntry, CacheEntryMap>();
        }
    }
}