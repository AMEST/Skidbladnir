using Microsoft.EntityFrameworkCore;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public abstract class DbContextBase : DbContext
    {

        private readonly DbContextOptions _options;

        protected DbContextBase(DbContextOptions options)
            : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypeDefinitionExtension = _options.FindExtension<EntityTypeDefinitionsExtension>();

            foreach (var entityTypeDefinition in entityTypeDefinitionExtension.EntityTypeDefinitions)
                entityTypeDefinition.Configure(modelBuilder);
        }
    }
}