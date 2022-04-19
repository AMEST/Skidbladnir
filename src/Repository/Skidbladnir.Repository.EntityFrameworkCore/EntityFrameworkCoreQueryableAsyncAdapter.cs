using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public class EntityFrameworkCoreQueryableAsyncAdapter : IQueryableAsyncAdapter
    {
        /// <inheritdoc />
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query) =>
            EntityFrameworkQueryableExtensions.ToListAsync(query);

        /// <inheritdoc />
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) =>
            EntityFrameworkQueryableExtensions.ToListAsync(query, cancellationToken);

        /// <inheritdoc />
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query) =>
            EntityFrameworkQueryableExtensions.ToArrayAsync(query);

        /// <inheritdoc />
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) =>
            EntityFrameworkQueryableExtensions.ToArrayAsync(query, cancellationToken);

        /// <inheritdoc />
        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector) =>
            EntityFrameworkQueryableExtensions.ToDictionaryAsync(source, keySelector);

        /// <inheritdoc />
        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector, CancellationToken cancellationToken) =>
            EntityFrameworkQueryableExtensions.ToDictionaryAsync(source, keySelector, cancellationToken);

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
            EntityFrameworkQueryableExtensions.ToDictionaryAsync(source, keySelector, elementSelector);

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken) =>
            EntityFrameworkQueryableExtensions.ToDictionaryAsync(source, keySelector, elementSelector, cancellationToken);

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query) =>
            EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query);

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) =>
            EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, cancellationToken);

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter) =>
            EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, filter);

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken) =>
            EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, filter, cancellationToken);

        /// <inheritdoc />
        public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
            EntityFrameworkQueryableExtensions.CountAsync(query, cancellationToken);

        /// <inheritdoc />
        public Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            EntityFrameworkQueryableExtensions.CountAsync(query, predicate, cancellationToken);

        /// <inheritdoc />
        public Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
            EntityFrameworkQueryableExtensions.AnyAsync(query, cancellationToken);

        /// <inheritdoc />
        public Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            EntityFrameworkQueryableExtensions.AnyAsync(query, predicate, cancellationToken);

        /// <inheritdoc />
        public IQueryable<T> Include<T>(IQueryable<T> query, string path) where T : class =>
            EntityFrameworkQueryableExtensions.Include(query, path);

        /// <inheritdoc />
        public IQueryable<T> Include<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path) where T : class =>
            EntityFrameworkQueryableExtensions.Include(query, path);

        /// <inheritdoc />
        public bool IsQueryableSupported<T>(IQueryable<T> query) => query.Provider is IAsyncQueryProvider;

    }
}