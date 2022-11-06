using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Skidbladnir.Repository.Abstractions;
using Skidbladnir.Utility.Common;

namespace Skidbladnir.Repository.MongoDB
{
    internal class MongoRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IHasId<string>
        where TDbContext : class, IMongoDbContext

    {
        private readonly TDbContext _mongoContext;
        private readonly IMongoDbContextConfiguration _configuration;
        private readonly IMongoCollection<TEntity> _dbCollection;

        public MongoRepository(TDbContext context, MongoDbContextConfiguration<TDbContext> configuration)
        {
            _mongoContext = context;
            _configuration = configuration;
            _dbCollection = _mongoContext.GetCollection<TEntity>();
        }

        public Task Create(TEntity obj, CancellationToken cancellationToken = default)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }

            return Retry.Do(() => _dbCollection.InsertOneAsync(obj, null, cancellationToken), _configuration.RetryCount);
        }

        public Task Delete(TEntity obj, CancellationToken cancellationToken = default)
        {
            return Retry.Do(() => _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(x => x.Id, obj.Id), cancellationToken),
                _configuration.RetryCount);
        }

        public virtual Task Update(TEntity obj, CancellationToken cancellationToken = default)
        {
            return Retry.Do(() => _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(x => x.Id, obj.Id), obj,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, cancellationToken), _configuration.RetryCount);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbCollection.AsQueryable();
        }
    }
}