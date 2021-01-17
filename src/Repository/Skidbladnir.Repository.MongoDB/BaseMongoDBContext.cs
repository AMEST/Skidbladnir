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
        public virtual IMongoCollection<T> GetCollection<T>(string name)
        {
            return Db.GetCollection<T>(name);
        }
    }
}