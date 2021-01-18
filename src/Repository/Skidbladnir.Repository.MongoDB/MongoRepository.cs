using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.MongoDB
{
    internal class MongoRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class, IHasId
        where TDbContext : class, IMongoDbContext

    {
        private readonly TDbContext _mongoContext;
        private IMongoCollection<TEntity> _dbCollection;

        public MongoRepository(TDbContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>();
        }

        public async Task Create(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }

            if (string.IsNullOrEmpty(obj.Id))
                obj.Id = ObjectId.GenerateNewId().ToString();

            await _dbCollection.InsertOneAsync(obj);
        }

        public void Delete(string id)
        {
            _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));
        }

        public virtual void Update(TEntity obj)
        {
            _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.Id), obj, new ReplaceOptions()
            {
                IsUpsert = true
            });
        }

        public async Task<TEntity> Get(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);

            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbCollection.AsQueryable();
        }
    }
}