using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Modules;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    public class DistributedCacheMongoModule : Module
    {
        public override void Configure(IServiceCollection services)
        {
            var configuration = Configuration.Get<DistributedCacheMongoModuleConfiguration>();
            services.AddMongoDistributedCache(configuration);
        }
    }
}