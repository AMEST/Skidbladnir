using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Skidbladnir.Repository.Abstractions;
using Skidbladnir.Utility.Common;

namespace Skidbladnir.Repository.MongoDB
{
    internal class MongoRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IHasId
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

        public Task Create(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }

            if (string.IsNullOrEmpty(obj.Id))
                obj.Id = ObjectId.GenerateNewId().ToString();

            return Retry.Do(() => _dbCollection.InsertOneAsync(obj), _configuration.RetryCount);
        }

        public Task Delete(string id)
        {
            return Retry.Do(() => _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)),
                _configuration.RetryCount);
        }

        public virtual Task Update(TEntity obj)
        {
            return Retry.Do(() => _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.Id), obj,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }), _configuration.RetryCount);
        }

        public Task<TEntity> Get(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);

            return Retry.Do(() => _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync(),
                _configuration.RetryCount);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbCollection.AsQueryable();
        }
    }
}