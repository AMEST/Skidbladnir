using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    public class Repository<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IHasId<int>
    {
        private readonly TDbContext _context;
        private DbSet<TEntity> _entities;

        public Repository(TDbContext context)
        {
            _context = context;
        }

        protected DbSet<TEntity> Entities => _entities ?? (_entities = _context.Set<TEntity>());

        public async Task Create(TEntity obj, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(obj);

            await Entities.AddAsync(obj, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CreateAll(IEnumerable<TEntity> objs, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(objs);

            var entitiesList = objs as IList<TEntity> ?? objs.ToList();
            
            if (entitiesList.Count == 0) return;
            
            await Entities.AddRangeAsync(entitiesList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(TEntity obj, CancellationToken cancellationToken = default)
        {
            Entities.Update(obj);
            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Delete(TEntity obj, CancellationToken cancellationToken = default)
        {
            Entities.Remove(obj);
            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAll(IEnumerable<TEntity> objs, CancellationToken cancellationToken = default)
        {
            var entitiesList = objs as IList<TEntity> ?? objs.ToList();
            
            if (entitiesList.Count == 0) return;

            Entities.RemoveRange(entitiesList);
            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAll(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            var items = await EntityFrameworkQueryableExtensions.ToListAsync(Entities.Where(filter), cancellationToken)
                .ConfigureAwait(false);
            await DeleteAll(items, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }
    }
}