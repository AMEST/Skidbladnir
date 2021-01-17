using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Skidbladnir.DataProtection.Abstractions;
using Skidbladnir.Repository.MongoDB;

namespace Skidbladnir.DataProtection.MongoDb
{
    public static class MongoDbContextBuilderExtensions
    {
        public static IMongoDbContextBuilder UseDataProtection(this IMongoDbContextBuilder builder,
            IServiceCollection services)
        {
            services.AddSingleton<IXmlRepository, XmlRepository>()
                .AddSingleton<IConfigureOptions<KeyManagementOptions>, DataProtectionOptionsConfigurator>();
            return builder.AddEntity<DbXmlKey, DbXmlKeyMap>();
        }
    }
}