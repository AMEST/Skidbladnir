using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Repository.EntityFrameworkCore
{
    internal class Repository<TDbContext, TEntity> : IRepository<TEntity>
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
            await Entities.AddAsync(obj);
            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
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

        /// <inheritdoc />
        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }
    }
}