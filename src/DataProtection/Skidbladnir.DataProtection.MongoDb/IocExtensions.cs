using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;

namespace Skidbladnir.DataProtection.MongoDb
{
    public static class IocExtensions
    {
        /// <summary>
        /// Connecting the data protection module with using BaseMongoDbContext
        /// </summary>
        public static IDataProtectionBuilder PersistKeysToMongoDb(this IDataProtectionBuilder builder,
            string connectionString, string collectionName = null)
        {
            
            builder.Services.AddDataProtectionMongoDb(connectionString, collectionName);
            return builder;
        }

        /// <summary>
        /// Connecting the data protection module with using BaseMongoDbContext
        /// </summary>
        public static IDataProtectionBuilder PersistKeysToMongoDb(this IDataProtectionBuilder builder,
            DataProtectionMongoModuleConfiguration configuration)
        {

            builder.Services.AddDataProtectionMongoDb(configuration);
            return builder;
        }

        public static IServiceCollection AddDataProtectionMongoDb(this IServiceCollection services,
            string connectionString, string collectionName = null)
        {
            var configuration = new DataProtectionMongoModuleConfiguration{ConnectionString = connectionString};
            if (!string.IsNullOrWhiteSpace(collectionName))
                configuration.CollectionName = collectionName;
            return services.AddDataProtectionMongoDb(configuration);
        }

        public static IServiceCollection AddDataProtectionMongoDb(this IServiceCollection services,
            DataProtectionMongoModuleConfiguration configuration)
        {
            BsonClassMap.RegisterClassMap(new DbXmlKeyMap());
            return services.AddSingleton(configuration)
                .AddSingleton<MongoDbContext>()
                .AddSingleton<IXmlRepository, XmlRepository>()
                .AddSingleton<IConfigureOptions<KeyManagementOptions>, DataProtectionOptionsConfigurator>();
        }
    }
}