using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Repository.Abstractions
{
    /// <summary>
    ///     Async methods adapter for IQueryable
    /// </summary>
    public interface IQueryableAsyncAdapter
    {
        /// <summary>
        ///     Asynchronously creates a <see cref="List{T}" /> from an <see cref="IQueryable" /> by enumerating it
        ///     asynchronously.
        /// </summary>
        Task<List<T>> ToListAsync<T>(IQueryable<T> query);

        /// <summary>
        ///     Asynchronously creates a <see cref="List{T}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously.
        /// </summary>
        Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously creates an array from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
        /// </summary>
        Task<T[]> ToArrayAsync<T>(IQueryable<T> query);

        /// <summary>
        ///     Asynchronously creates an array from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
        /// </summary>
        Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function.
        /// </summary>
        Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector);

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function.
        /// </summary>
        Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector, CancellationToken cancellationToken);

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function and a comparer.
        /// </summary>
        Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector);

        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}" /> from an <see cref="IQueryable{T}" /> by enumerating it
        ///     asynchronously
        ///     according to a specified key selector function and a comparer.
        /// </summary>
        Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IQueryable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);

        /// <summary>
        ///     Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously returns the first element of a sequence that satisfies a specified condition
        ///     or a default value if no such element is found.
        /// </summary>
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter);

        /// <summary>
        ///     Asynchronously returns the first element of a sequence that satisfies a specified condition
        ///     or a default value if no such element is found.
        /// </summary>
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Asynchronously returns the number of elements in a sequence.
        /// </summary>
        Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Asynchronously returns the number of elements in a sequence that satisfy a condition.
        /// </summary>
        Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Asynchronously determines whether a sequence contains any elements.
        /// </summary>
        Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Asynchronously determines whether any element of a sequence satisfies a condition.
        /// </summary>
        Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is
        ///     specified starting with the type of entity being queried (<typeparamref name="TEntity" />). Further
        ///     navigation properties to be included can be appended, separated by the '.' character.
        /// </summary>
        IQueryable<T> Include<T>(IQueryable<T> query, string path) where T : class;

        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is specified starting with the
        ///     type of entity being queried (<typeparamref name="TEntity" />). If you wish to include additional types based on the navigation
        ///     properties of the type being included, then chain a call to
        ///     <see
        ///         cref="ThenInclude{TEntity, TPreviousProperty, TProperty}(IIncludableQueryable{TEntity, IEnumerable{TPreviousProperty}}, Expression{Func{TPreviousProperty, TProperty}})" />
        ///     after this call.
        /// </summary>
        IQueryable<T> Include<T, TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path) where T : class;

        /// <summary>
        ///     Detect, can IQueryable support command
        /// </summary>
        bool IsQueryableSupported<T>(IQueryable<T> query);

    }
}