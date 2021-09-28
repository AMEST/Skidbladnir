﻿using System.Linq;
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

        public async Task Create(TEntity obj)
        {
            await Entities.AddAsync(obj);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task Update(TEntity obj)
        {
            Entities.Update(obj);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task Delete(TEntity obj)
        {
            Entities.Remove(obj);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }
    }
}