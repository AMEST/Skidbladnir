using MongoDB.Driver;

namespace Skidbladnir.Repository.MongoDB
{
    public interface IMongoDbContext
    {
        /// <summary>
        /// Get collection from mongodb
        /// </summary>
        IMongoCollection<T> GetCollection<T>();
    }
}