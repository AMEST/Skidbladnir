namespace Skidbladnir.Repository.MongoDB
{
    /// <summary>
    /// Configuration for DefaultMongoDbContext
    /// </summary>
    public interface IMongoDbContextConfiguration
    {
        /// <summary>
        /// Mongo Db Connection string
        /// </summary>
        string ConnectionString { get; set; }
    }
}