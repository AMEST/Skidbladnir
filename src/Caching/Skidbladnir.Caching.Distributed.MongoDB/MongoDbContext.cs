using System;
using MongoDB.Driver;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class MongoDbContext
    {
        private readonly MongoUrl _url;
        private readonly Lazy<IMongoClient> _client;
        private readonly Lazy<IMongoDatabase> _db;

        public MongoDbContext(DistributedCacheMongoModuleConfiguration configuration)
        {
            _url = MongoUrl.Create(configuration.ConnectionString);
            _client = new Lazy<IMongoClient>(CreateClient);
            _db = new Lazy<IMongoDatabase>(GetDataBase);
        }
        
        public IMongoCollection<CacheEntry> GetCollection()
        {
            return _db.Value.GetCollection<CacheEntry>("DistributedCache");
        }
        
        private IMongoClient CreateClient()
        {
            return new MongoClient(_url);
        }

        private IMongoDatabase GetDataBase()
        {
            return _client.Value.GetDatabase(_url.DatabaseName);
        }
    }
}