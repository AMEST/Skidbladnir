using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public static class IoCExtensions
    {
        public static IServiceCollection RegisterContext<TContext>(
            this IServiceCollection collection,
            Action<DbContextOptionsBuilder<TContext>> optionsConfigure,
            Action<IContextBuilder<TContext>> entitiesConfigure)
            where TContext : DbContext
        {
            QueryableAsyncExtensions.TryAddAdapter<EntityFrameworkCoreQueryableAsyncAdapter>();
            EnsureDbContextConstructor<TContext>();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsConfigure(dbContextOptionsBuilder);

            IContextBuilder<TContext> entitiesConfigurationBuilder = new ContextBuilder<TContext>(collection);
            entitiesConfigure(entitiesConfigurationBuilder);
            ((IDbContextOptionsBuilderInfrastructure)dbContextOptionsBuilder)
                .AddOrUpdateExtension(((ContextBuilder<TContext>)entitiesConfigurationBuilder).Build());

            collection.AddScoped(r => (DbContextOptions<TContext>) dbContextOptionsBuilder.Options);
            collection.AddScoped(r =>
            {
                var config = r.GetService<DbContextOptions<TContext>>();
                return (TContext) Activator.CreateInstance(typeof(TContext), config);
            });

            return collection;
        }

        private static void EnsureDbContextConstructor<TContext>() where TContext : DbContext
        {
            var dbContextType = typeof(TContext);
            var configurationType = typeof(DbContextOptions<TContext>);
            var constructor = dbContextType.GetConstructor(new[] { configurationType });
            if (constructor == null)
                throw new InvalidOperationException(
                    $"Not found public constructor for type {dbContextType.FullName}, with argument {configurationType.FullName}");
        }

    }
}