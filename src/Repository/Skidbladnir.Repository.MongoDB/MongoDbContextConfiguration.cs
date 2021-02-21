namespace Skidbladnir.Repository.MongoDB
{
    public class MongoDbContextConfiguration<TDbContext> : IMongoDbContextConfiguration where TDbContext : class, IMongoDbContext
    {
        public string ConnectionString { get; set; }
        public int RetryCount { get; set; } = 1;
    }
}