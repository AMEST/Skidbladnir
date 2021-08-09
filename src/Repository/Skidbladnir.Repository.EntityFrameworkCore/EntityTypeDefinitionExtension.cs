using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    internal class EntityTypeDefinitionsExtension : IDbContextOptionsExtension
    {
        private readonly Dictionary<Type, IEntityTypeDefinition> _entityTypeDefinitions =
            new Dictionary<Type, IEntityTypeDefinition>();

        public IEnumerable<IEntityTypeDefinition> EntityTypeDefinitions => _entityTypeDefinitions.Values;

        public EntityTypeDefinitionsExtension Define<TEntity>()
            where TEntity : class
        {
            _entityTypeDefinitions[typeof(TEntity)] = new EntityTypeDefinition<TEntity>();
            return this;
        }

        public EntityTypeDefinitionsExtension Define<TEntity, TConfiguration>()
            where TConfiguration : IEntityTypeConfiguration<TEntity>, new()
            where TEntity : class
        {
            _entityTypeDefinitions[typeof(TEntity)] = new EntityTypeDefinition<TEntity, TConfiguration>();
            return this;
        }

        public void ApplyServices(IServiceCollection services)
        {
        }

        public void Validate(IDbContextOptions options)
        {
        }

        public DbContextOptionsExtensionInfo Info => new EntityTypeDefinitionsExtensionInfo(this);

        private class EntityTypeDefinitionsExtensionInfo : DbContextOptionsExtensionInfo
        {
            public EntityTypeDefinitionsExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override long GetServiceProviderHashCode()
            {
                return 0;
            }

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
            }

            public override bool IsDatabaseProvider { get; } = false;

            public override string LogFragment { get; } = "define entities";
        }
    }
}