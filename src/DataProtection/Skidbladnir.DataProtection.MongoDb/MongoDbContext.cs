using System;
using MongoDB.Driver;

namespace Skidbladnir.DataProtection.MongoDb
{
    internal class MongoDbContext
    {
        private readonly DataProtectionMongoModuleConfiguration _configuration;
        private readonly MongoUrl _url;
        private readonly Lazy<IMongoClient> _client;
        private readonly Lazy<IMongoDatabase> _db;

        public MongoDbContext(DataProtectionMongoModuleConfiguration configuration)
        {
            _configuration = configuration;
            _url = MongoUrl.Create(_configuration.ConnectionString);
            _client = new Lazy<IMongoClient>(CreateClient);
            _db = new Lazy<IMongoDatabase>(GetDataBase);
        }

        public IMongoCollection<DbXmlKey> GetCollection()
        {
            return _db.Value.GetCollection<DbXmlKey>(_configuration.CollectionName ?? "dataProtection");
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