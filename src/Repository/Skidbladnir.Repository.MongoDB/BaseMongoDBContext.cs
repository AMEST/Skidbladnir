using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Skidbladnir.Repository.MongoDB
{
    /// <summary>
    /// Base implementation of IMongoDbContext.
    /// Recommendation for use if using only one instance of mongodb
    /// If application connect to 2 (or more) different mongodb instance, create child class for each instance
    /// </summary>
    public class BaseMongoDbContext : IMongoDbContext
    {
        private IMongoDatabase Db { get; set; }
        private MongoClient MongoClient { get; set; }

        public BaseMongoDbContext(IMongoDbContextConfiguration configuration)
        {
            var url = MongoUrl.Create(configuration.ConnectionString);
            MongoClient = new MongoClient(url);
            Db = MongoClient?.GetDatabase(url.DatabaseName);
        }

        /// <inheritdoc />
        public virtual IMongoCollection<T> GetCollection<T>()
        {
            var cm = BsonClassMap.GetRegisteredClassMaps().OfType<EntityMapClass<T>>().SingleOrDefault();
            if (cm == null)
            {
                throw new Exception($"No registered EntityMapClass<T> found for type: {typeof(T).Name}");
            }
            if (string.IsNullOrEmpty(cm.CollectionName))
            {
                throw new Exception($"EntityMapClass<T> must call ToCollection method to specify mongo collection name. Type: {typeof(T).Name}");
            }
            return Db.GetCollection<T>(cm.CollectionName);
        }
    }
}