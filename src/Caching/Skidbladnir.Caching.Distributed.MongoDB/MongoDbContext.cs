using MongoDB.Driver;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class MongoDbContext
    {
        private IMongoDatabase Db { get; }
        private MongoClient MongoClient { get; }

        public MongoDbContext(string connectionString)
        {
            var url = MongoUrl.Create(connectionString);
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