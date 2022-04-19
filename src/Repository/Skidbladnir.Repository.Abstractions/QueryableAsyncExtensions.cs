using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Repository.Abstractions
{
    public static class QueryableAsyncExtensions
    {
        private static bool _fallbackAdapterEnabled = false;
        private static readonly IQueryableAsyncAdapter FallbackAdapter = new FallbackQueryableAsyncAdapter();
        private static IList<IQueryableAsyncAdapter> Adapters { get; set; } = new List<IQueryableAsyncAdapter>();

        /// <summary>
        ///     Asynchronously creates a <see cref="List{T}" /> from an <see cref="IQueryable" /> by enumerating it
        ///     asynchronously.
        /// </summary>
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToListAsync(source);
        }

        /// <summary>
        ///     Asynchronously creates a <see cref="List{T}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously.
        /// </summary>
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToListAsync(source, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously creates an array from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
        /// </summary>
        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> source)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToArrayAsync(source);
        }

        /// <summary>
        ///     Asynchronously creates an array from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
        /// </summary>
        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToArrayAsync(source, cancellationToken);
        }

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function.
        /// </summary>
        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQueryable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToDictionaryAsync(source, keySelector);
        }

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function.
        /// </summary>
        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IQueryable<TSource> source,
            Func<TSource, TKey> keySelector,
            CancellationToken cancellationToken)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToDictionaryAsync(source, keySelector, cancellationToken);
        }

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function and a comparer.
        /// </summary>
        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
            this IQueryable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToDictionaryAsync(source, keySelector, elementSelector);
        }

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function and a comparer.
        /// </summary>
        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
            this IQueryable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            CancellationToken cancellationToken)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.ToDictionaryAsync(source, keySelector, elementSelector, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.FirstOrDefaultAsync(source);
        }

        /// <summary>
        ///     Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.FirstOrDefaultAsync(source, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously returns the first element of a sequence that satisfies a specified condition
        ///     or a default value if no such element is found.
        /// </summary>
        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> filter)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.FirstOrDefaultAsync(source, filter);
        }

        /// <summary>
        ///     Asynchronously returns the first element of a sequence that satisfies a specified condition
        ///     or a default value if no such element is found.
        /// </summary>
        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.FirstOrDefaultAsync(source, filter, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously returns the number of elements in a sequence.
        /// </summary>
        public static Task<int> CountAsync<T>(this IQueryable<T> source,
            CancellationToken cancellationToken = default)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.CountAsync(source, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously returns the number of elements in a sequence that satisfy a condition.
        /// </summary>
        public static Task<int> CountAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.CountAsync(source, predicate, cancellationToken);
        }

        /// <summary>
        /// Asynchronously determines whether a sequence contains any elements.
        /// </summary>
        public static Task<bool> AnyAsync<T>(this IQueryable<T> source,
            CancellationToken cancellationToken = default)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.AnyAsync(source, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously determines whether any element of a sequence satisfies a condition.
        /// </summary>
        public static Task<bool> AnyAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.AnyAsync(source, predicate, cancellationToken);
        }

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is
        ///     specified starting with the type of entity being queried (<typeparamref name="TEntity" />). Further
        ///     navigation properties to be included can be appended, separated by the '.' character.
        /// </summary>
        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path) where T : class
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.Include(source, path);
        }

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is specified starting with the
        ///     type of entity being queried (<typeparamref name="TEntity" />). If you wish to include additional types based on the navigation
        ///     properties of the type being included, then chain a call to
        ///     <see
        ///         cref="ThenInclude{TEntity, TPreviousProperty, TProperty}(IIncludableQueryable{TEntity, IEnumerable{TPreviousProperty}}, Expression{Func{TPreviousProperty, TProperty}})" />
        ///     after this call.
        /// </summary>
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source,
            Expression<Func<T, TProperty>> path)
            where T : class
        {
            var adapter = GetSupportedAdapter(source);
            return adapter.Include(source, path);
        }

        /// <summary>
        ///     Enable fallback async adapter
        /// </summary>
        public static void EnableFallback()
        {
            _fallbackAdapterEnabled = true;
        }

        /// <summary>
        ///     Add adapter if not exists
        /// </summary>
        public static void TryAddAdapter<T>()
            where T : IQueryableAsyncAdapter, new()
        {
            if(Adapters.Any(x => x.GetType() == typeof(T)))
                return;

            Adapters.Add(new T());
        }

        private static IQueryableAsyncAdapter GetSupportedAdapter<T>(IQueryable<T> source)
        {
            foreach (var adapter in Adapters)
            {
                if (!adapter.IsQueryableSupported(source))
                    continue;

                return adapter;
            }

            if (_fallbackAdapterEnabled)
                return FallbackAdapter;

            throw new NotSupportedException(
                $"Cannot execute command asynchronously for queryable {source.GetType().FullName}. Enable fallback adapter `QueryableAsyncExtensions.EnableFallback()`");
        }

    }
}