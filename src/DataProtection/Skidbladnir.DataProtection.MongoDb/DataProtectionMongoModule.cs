using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Modules;

namespace Skidbladnir.DataProtection.MongoDb
{
    public class DataProtectionMongoModule : Module
    {
        public override void Configure(IServiceCollection services)
        {
            var configuration = Configuration.Get<DataProtectionMongoModuleConfiguration>();
            services.AddDataProtectionMongoDb(configuration);
        }
    }
}