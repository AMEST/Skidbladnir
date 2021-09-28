using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.MongoDB
{
    /// <summary>
    /// Configuring MongoDbContext settings and Entities
    /// </summary>
    public class MongoDbContextBuilder<TDbContext> : IMongoDbContextBuilder where TDbContext : class, IMongoDbContext
    {
        private readonly IServiceCollection _services;
        private readonly MongoDbContextConfiguration<TDbContext> _configuration;

        public MongoDbContextBuilder(IServiceCollection services, MongoDbContextConfiguration<TDbContext> configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public IMongoDbContextBuilder AddEntity<TEntity>() where TEntity : class, IHasId<string>
        {
            _services.AddSingleton<IRepository<TEntity>, MongoRepository<TEntity, TDbContext>>();
            var defaultEntityConfiguration = Utilities.CreateDefaultMongoMap<TEntity>();
            BsonClassMap.RegisterClassMap(defaultEntityConfiguration);
            return this;
        }

        /// <inheritdoc />
        public IMongoDbContextBuilder AddEntity<TEntity, TEntityConfiguration>()
        where TEntity: class, IHasId<string>
        where TEntityConfiguration : EntityMapClass<TEntity>, new()
        {
            _services.AddSingleton<IRepository<TEntity>, MongoRepository<TEntity,TDbContext>>();
            BsonClassMap.RegisterClassMap(new TEntityConfiguration());
            return this;
        }

        /// <inheritdoc />
        public IMongoDbContextBuilder UseConnectionString(string connectionString)
        {
            _configuration.ConnectionString = connectionString;
            return this;
        }

        /// <inheritdoc />
        public IMongoDbContextBuilder UseRetry(int retryCount)
        {
            if(retryCount < 1)
                throw new ArgumentException("Can't be less 1", nameof(retryCount));
            _configuration.RetryCount = retryCount;
            return this;
        }
    }
}