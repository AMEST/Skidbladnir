using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.MongoDB
{
    public class MongoQueryableAsyncAdapter : IQueryableAsyncAdapter
    {
        /// <inheritdoc />
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            var mongoQueryable = GetMongoQueryable(query);

            return IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable);
        }

        /// <inheritdoc />
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        {
            var mongoQueryable = GetMongoQueryable(query);
            return IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
        {
            var mongoQueryable = GetMongoQueryable(query);
            return (await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable)).ToArray();
        }

        /// <inheritdoc />
        public async Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        {
            var mongoQueryable = GetMongoQueryable(query);
            return (await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable, cancellationToken)).ToArray();
        }

        /// <inheritdoc />
        public async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> query,
            Func<TSource, TKey> keySelector)
        {
            var mongoQueryable = GetMongoQueryable(query);

            var list = await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable);

            return list.ToDictionary(keySelector);
        }

        /// <inheritdoc />
        public async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> query,
            Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            var mongoQueryable = GetMongoQueryable(query);

            var list = await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable, cancellationToken);

            return list.ToDictionary(keySelector);
        }

        /// <inheritdoc />
        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
            IQueryable<TSource> query, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var mongoQueryable = GetMongoQueryable(query);

            var list = await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable);

            return list.ToDictionary(keySelector, elementSelector);
        }

        /// <inheritdoc />
        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
            IQueryable<TSource> query, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            CancellationToken cancellationToken)
        {
            var mongoQueryable = GetMongoQueryable(query);

            var list = await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable, cancellationToken);

            return list.ToDictionary(keySelector, elementSelector);
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter)
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.FirstOrDefaultAsync(filter);
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken)
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.FirstOrDefaultAsync(filter, cancellationToken);
        }

        /// <inheritdoc />
        public Task<int> CountAsync<T>(IQueryable<T> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.CountAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.CountAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> AnyAsync<T>(IQueryable<T> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var mongoQueryable = GetMongoQueryable(query);

            return IAsyncCursorSourceExtensions.AnyAsync(mongoQueryable, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var mongoQueryable = GetMongoQueryable(query);

            return mongoQueryable.AnyAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public IQueryable<T> Include<T>(IQueryable<T> query, string path) where T : class
        {
            throw new NotSupportedException("MongoDB not supported Include");
        }

        /// <inheritdoc />
        public IQueryable<T> Include<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path)
            where T : class
        {
            throw new NotSupportedException("MongoDB not supported Include");
        }

        /// <inheritdoc />
        public bool IsQueryableSupported<T>(IQueryable<T> query)
        {
            return query is IMongoQueryable<T>;
        }

        private static IMongoQueryable<T> GetMongoQueryable<T>(IQueryable<T> query)
        {
            return (IMongoQueryable<T>)query;
        }

    }
}