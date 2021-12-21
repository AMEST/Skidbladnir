using MongoDB.Driver;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class MongoDbContext
    {
        private readonly DistributedCacheMongoModuleConfiguration _configuration;
        private IMongoDatabase Db { get; }
        private MongoClient MongoClient { get; }

        public MongoDbContext(DistributedCacheMongoModuleConfiguration configuration)
        {
            _configuration = configuration;
            var url = MongoUrl.Create(configuration.ConnectionString);
            MongoClient = new MongoClient(url);
            Db = MongoClient?.GetDatabase(url.DatabaseName);
        }

        /// <inheritdoc />
        public IMongoCollection<CacheEntry> GetCollection()
        {
            return Db.GetCollection<CacheEntry>("DistributedCache");
        }
    }
}