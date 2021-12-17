using MongoDB.Driver;

namespace Skidbladnir.DataProtection.MongoDb
{
    internal class MongoDbContext
    {
        private readonly DataProtectionMongoModuleConfiguration _configuration;

        private IMongoDatabase Db { get; }
        private MongoClient MongoClient { get; }

        public MongoDbContext(DataProtectionMongoModuleConfiguration configuration)
        {
            _configuration = configuration;
            var url = MongoUrl.Create(configuration.ConnectionString);
            MongoClient = new MongoClient(url);
            Db = MongoClient?.GetDatabase(url.DatabaseName);
        }

        /// <inheritdoc />
        public IMongoCollection<DbXmlKey> GetCollection()
        {
            return Db.GetCollection<DbXmlKey>(_configuration.CollectionName ?? "dataProtection");
        }
    }
}