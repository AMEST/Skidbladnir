using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public class ContextBuilder<TContext> : IContextBuilder<TContext> where TContext : DbContext
    {
        private readonly IServiceCollection _collection;
        private readonly EntityTypeDefinitionsExtension _entityTypeDefinitionsExtension;

        public ContextBuilder(IServiceCollection collection)
        {
            _collection = collection;
            _entityTypeDefinitionsExtension = new EntityTypeDefinitionsExtension();
        }

        public IContextBuilder<TContext> AddEntity<TEntity>() where TEntity : class, IHasId<int>
        {
            _collection.AddScoped<IRepository<TEntity>, Repository<TContext, TEntity>>();
            _entityTypeDefinitionsExtension.Define<TEntity>();
            return this;
        }

        public IContextBuilder<TContext> AddEntity<TEntity, TConfiguration>() where TEntity : class, IHasId<int>
            where TConfiguration : IEntityTypeConfiguration<TEntity>, new()
        {
            AddEntity<TEntity>();
            _entityTypeDefinitionsExtension.Define<TEntity, TConfiguration>();
            return this;
        }


        internal EntityTypeDefinitionsExtension Build()
        {
            return _entityTypeDefinitionsExtension;
        }
    }
}