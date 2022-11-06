using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class MongoDbCache : IDistributedCache, IDisposable
    {
        private readonly TimeSpan _expiredItemsTimerPeriod = TimeSpan.FromMinutes(10);
        private readonly MongoDbContext _dbContext;
        private readonly Timer _removeExpiredItemsTimer;

        public MongoDbCache(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
            _removeExpiredItemsTimer =
                new Timer(
                    callback: RemoveExpired,
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: _expiredItemsTimerPeriod);
        }

        internal IMongoCollection<CacheEntry> Collection => _dbContext.GetCollection();

        public byte[] Get(string key)
        {
            return GetAsync(key)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
        {
            var cachedEntry = await Collection.AsQueryable()
                .Where(i => i.Id == key)
                .SingleOrDefaultAsync(token)
                .ConfigureAwait(false);

            if (cachedEntry == null)
                return null;

            if (cachedEntry.IsExpired())
                return null;

            if (!cachedEntry.IsRefreshNeeded())
                return cachedEntry.Value;

            await Collection.ReplaceOneAsync(Builders<CacheEntry>.Filter.Eq(x => x.Id, cachedEntry.Id), cachedEntry,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, token)
                .ConfigureAwait(false);

            return cachedEntry.Value;

        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            SetAsync(key, value, options)
                .GetAwaiter()
                .GetResult();
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = new CancellationToken())
        {
            var cachingEntry = new CacheEntry(key)
            {
                Value = value,
                AbsoluteExpiration = options.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = options.SlidingExpiration,
                CreationDateTimeOffset = DateTimeOffset.UtcNow
            };
            await Collection.ReplaceOneAsync(Builders<CacheEntry>.Filter.Eq(x => x.Id, cachingEntry.Id), cachingEntry,
                new ReplaceOptions()
                {
                    IsUpsert = true
                }, token);
        }

        public void Refresh(string key)
        {
            RefreshAsync(key)
                .GetAwaiter()
                .GetResult();
        }

        public Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
        {
            return GetAsync(key, token);
        }

        public void Remove(string key)
        {
            RemoveAsync(key)
                .GetAwaiter()
                .GetResult();
        }

        public async Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
        {
            var cachedEntry = Collection.AsQueryable().Where(i => i.Id == key).SingleOrDefaultAsync(token);
            if (cachedEntry != null)
                await Collection.DeleteOneAsync(Builders<CacheEntry>.Filter.Eq(x => x.Id, key), token);
        }

        public void Dispose()
        {
            _removeExpiredItemsTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            _removeExpiredItemsTimer?.Dispose();
        }

        private async void RemoveExpired(object state)
        {
            var cacheEntryList = await Collection.AsQueryable().ToListAsync();
            foreach (var cacheEntry in cacheEntryList)
            {
                if(!cacheEntry.IsExpired()) continue;
                await Collection.DeleteOneAsync(Builders<CacheEntry>.Filter.Eq(x => x.Id, cacheEntry.Id));
            }
        }
    }
}