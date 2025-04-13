using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Skidbladnir.Repository.Abstractions;
using Skidbladnir.Utility.Common;

namespace Skidbladnir.Repository.MongoDB
{
    public class MongoRepository<TEntity, TDbContext> : IRepository<TEntity>
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

        protected IMongoCollection<TEntity> Entities => _dbCollection;

        public Task Create(TEntity obj, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(obj);

            return Retry.Do(() => _dbCollection.InsertOneAsync(obj, null, cancellationToken), _configuration.RetryCount);
        }

        public Task CreateAll(IEnumerable<TEntity> objs, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(objs);

            var entitiesList = objs as IList<TEntity> ?? objs.ToList();
            if (entitiesList.Count != 0)
                return _dbCollection.InsertManyAsync(entitiesList, null, cancellationToken);

            return Task.CompletedTask;
        }


        public Task Delete(TEntity obj, CancellationToken cancellationToken = default)
        {
            return Retry.Do(() => _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(x => x.Id, obj.Id), cancellationToken),
                _configuration.RetryCount);
        }

        public async Task DeleteAll(IEnumerable<TEntity> objs, CancellationToken cancellationToken = default)
        {
            foreach (var obj in objs)
            {
                await Delete(obj, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public Task DeleteAll(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            return Retry.Do(() => _dbCollection.DeleteManyAsync(filter, cancellationToken));
        }

        public virtual Task Update(TEntity obj, CancellationToken cancellationToken = default)
        {
            return Retry.Do(() => _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq(x => x.Id, obj.Id), obj,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, cancellationToken),
                 _configuration.RetryCount);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbCollection.AsQueryable();
        }
    }
}