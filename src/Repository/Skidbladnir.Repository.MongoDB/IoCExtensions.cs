using System;
using Microsoft.Extensions.DependencyInjection;

namespace Skidbladnir.Repository.MongoDB
{
    /// <summary>
    /// Registration Extensions
    /// </summary>
    public static class IocExtensions
    {
        /// <summary>
        /// Register Base MongoDB Context for mongodb server
        /// </summary>
        public static IServiceCollection AddMongoDbContext(
            this IServiceCollection services,
            Action<MongoDbContextBuilder<BaseMongoDbContext>> builder)
        {
            return AddMongoDbContext<BaseMongoDbContext>(services, builder);
        }

        /// <summary>
        /// Register MongoDB Context for mongodb server
        /// With different MongoDbContext
        /// </summary>
        public static IServiceCollection AddMongoDbContext<TDbContext>(
            this IServiceCollection services,
            Action<MongoDbContextBuilder<TDbContext>> builder)
            where TDbContext : BaseMongoDbContext
        {
            var configuration = new MongoDbContextConfiguration<TDbContext>();
            var dbContextBuilder = new MongoDbContextBuilder<TDbContext>(services, configuration);
            builder?.Invoke(dbContextBuilder);

            services.AddSingleton<MongoDbContextConfiguration<TDbContext>>(configuration);

            services.AddSingleton<TDbContext>(r =>
            {
                var dbContextConfiguration = r.GetService<MongoDbContextConfiguration<TDbContext>>();
                return (TDbContext) Activator.CreateInstance(typeof(TDbContext),
                    (IMongoDbContextConfiguration) dbContextConfiguration);
            });

            return services;
        }
    }
}