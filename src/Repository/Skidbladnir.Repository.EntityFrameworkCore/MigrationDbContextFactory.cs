using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public abstract class MigrationDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContextBase
    {

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
            throw new NotImplementedException();
        }

        public TContext CreateDbContext(string[] args)
        {
            var collection = new ServiceCollection();
            ConfigureServices(collection);
            var provider = collection.BuildServiceProvider();
            return provider.GetService<TContext>();
        }
    }
}