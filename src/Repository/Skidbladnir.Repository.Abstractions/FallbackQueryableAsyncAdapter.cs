using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Repository.Abstractions
{
    public class FallbackQueryableAsyncAdapter : IQueryableAsyncAdapter
    {
        /// <inheritdoc />
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            return Task.FromResult(query.ToList());
        }

        /// <inheritdoc />
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        {
            return Task.FromResult(query.ToList());
        }

        /// <inheritdoc />
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
        {
            return Task.FromResult(query.ToArray());
        }

        /// <inheritdoc />
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        {
            return Task.FromResult(query.ToArray());
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> query,
            Func<TSource, TKey> keySelector)
        {
            return Task.FromResult(query.ToDictionary(keySelector));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> query,
            Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            return Task.FromResult(query.ToDictionary(keySelector));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> query,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return Task.FromResult(query.ToDictionary(keySelector, elementSelector));
        }

        /// <inheritdoc />
        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> query,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(query.ToDictionary(keySelector, elementSelector));
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            return Task.FromResult(query.FirstOrDefault());
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        {
            return Task.FromResult(query.FirstOrDefault());
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter)
        {
            return Task.FromResult(query.FirstOrDefault(filter));
        }

        /// <inheritdoc />
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(query.FirstOrDefault(filter));
        }

        /// <inheritdoc />
        public Task<int> CountAsync<T>(IQueryable<T> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(query.Count());
        }

        /// <inheritdoc />
        public Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(query.Count(predicate));
        }

        /// <inheritdoc />
        public Task<bool> AnyAsync<T>(IQueryable<T> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(query.Any());
        }

        /// <inheritdoc />
        public Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(query.Any(predicate));
        }

        /// <inheritdoc />
        public IQueryable<T> Include<T>(IQueryable<T> query, string path) where T : class
        {
            throw new NotSupportedException("Include not supported by fallback adapter");
        }

        /// <inheritdoc />
        public IQueryable<T> Include<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path)
            where T : class
        {
            throw new NotSupportedException("Include not supported by fallback adapter");
        }

        /// <inheritdoc />
        public bool IsQueryableSupported<T>(IQueryable<T> query)
        {
            return true;
        }
    }
}