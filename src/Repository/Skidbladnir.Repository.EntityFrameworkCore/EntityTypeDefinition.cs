using Microsoft.EntityFrameworkCore;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    internal class EntityTypeDefinition<TEntity> : IEntityTypeDefinition
        where TEntity : class
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entityType = typeof(TEntity);

            if (modelBuilder.Model.FindEntityType(entityType) == null)
                modelBuilder.Model.AddEntityType(typeof(TEntity));
        }
    }

    internal class EntityTypeDefinition<TEntity, TConfiguration> : IEntityTypeDefinition
        where TConfiguration : IEntityTypeConfiguration<TEntity>, new()
        where TEntity : class
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            new TConfiguration().Configure(modelBuilder.Entity<TEntity>());
        }
    }
}