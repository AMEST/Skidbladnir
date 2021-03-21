using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Skidbladnir.DataProtection.Abstractions;
using Skidbladnir.Repository.MongoDB;

namespace Skidbladnir.DataProtection.MongoDb
{
    public static class IocExtensions
    {
        /// <summary>
        /// Connecting the data protection module with using MongoContextBuilder
        /// </summary>
        public static IMongoDbContextBuilder UseDataProtection(this IMongoDbContextBuilder builder,
            IServiceCollection services)
        {
            services.AddSingleton<IXmlRepository, XmlRepository>()
                .AddSingleton<IConfigureOptions<KeyManagementOptions>, DataProtectionOptionsConfigurator>();
            return builder.AddEntity<DbXmlKey, DbXmlKeyMap>();
        }

        /// <summary>
        /// Connecting the data protection module with using BaseMongoDbContext
        /// </summary>
        public static IServiceCollection UseDataProtection(this IServiceCollection services)
        {
            return services.UseDataProtection<BaseMongoDbContext>();
        }

        /// <summary>
        /// Connecting the data protection module with using Custom Mongo db context
        /// </summary>
        public static IServiceCollection UseDataProtection<TDbContext>(this IServiceCollection services)
            where TDbContext : BaseMongoDbContext
        {
            services.AddSingleton<IXmlRepository, XmlRepository>()
                .AddSingleton<IConfigureOptions<KeyManagementOptions>, DataProtectionOptionsConfigurator>();
            services.ConfigureMongoDbContext<TDbContext>(b => b.AddEntity<DbXmlKey, DbXmlKeyMap>());
            return services;
        }
    }
}