﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Repository.Abstractions;

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
            QueryableAsyncExtensions.TryAddAdapter<MongoQueryableAsyncAdapter>();
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
        /// <summary>
        /// Addition register entities for base MongoDB context
        /// </summary>
        public static IServiceCollection ConfigureMongoDbContext(
            this IServiceCollection services,
            Action<MongoDbContextBuilder<BaseMongoDbContext>> builder)
        {
            var configuration = new MongoDbContextConfiguration<BaseMongoDbContext>();
            var dbContextBuilder = new MongoDbContextBuilder<BaseMongoDbContext>(services, configuration);
            builder?.Invoke(dbContextBuilder);
            return services;
        }

        /// <summary>
        /// Addition register entities for MongoDB context
        /// </summary>
        public static IServiceCollection ConfigureMongoDbContext<TDbContext>(
            this IServiceCollection services,
            Action<MongoDbContextBuilder<TDbContext>> builder)
            where TDbContext : BaseMongoDbContext
        {
            var configuration = new MongoDbContextConfiguration<TDbContext>();
            var dbContextBuilder = new MongoDbContextBuilder<TDbContext>(services, configuration);
            builder?.Invoke(dbContextBuilder);
            return services;
        }
    }
}