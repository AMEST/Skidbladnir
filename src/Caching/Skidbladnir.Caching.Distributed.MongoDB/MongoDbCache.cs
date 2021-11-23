using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class MongoDbCache : IDistributedCache, IDisposable
    {
        private readonly MongoDbContext _dbContext;
        private readonly Timer _clearExpiredItems;

        public MongoDbCache(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
            _clearExpiredItems =
                new Timer(
                    callback: RemoveExpired,
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromMinutes(10));
        }

        internal IMongoCollection<CacheEntry> Collection => _dbContext.GetCollection();

        public byte[] Get(string key)
        {
            return GetAsync(key).GetAwaiter().GetResult();
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
        {
            using (var cachedEntry = Collection.AsQueryable().SingleOrDefault(i => i.Id == key))
            {
                if (cachedEntry == null)
                    return null;

                if (cachedEntry.IsExpired())
                {
                    await Collection.DeleteOneAsync(Builders<CacheEntry>.Filter.Eq("_id", key), token);
                    return null;
                }

                await Collection.ReplaceOneAsync(Builders<CacheEntry>.Filter.Eq("_id", cachedEntry.Id), cachedEntry,
                    new ReplaceOptions()
                    {
                        IsUpsert = true
                    }, token);
                return cachedEntry.Value;
            }
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            SetAsync(key, value, options).Wait();
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = new CancellationToken())
        {
            using (var cachingEntry = new CacheEntry(key)
            {
                Value = value,
                AbsoluteExpiration = options.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = options.SlidingExpiration,
                CreationDateTimeOffset = DateTimeOffset.UtcNow
            })
            {
                await Collection.ReplaceOneAsync(Builders<CacheEntry>.Filter.Eq("_id", cachingEntry.Id), cachingEntry,
                    new ReplaceOptions()
                    {
                        IsUpsert = true
                    }, token);
            }
        }

        public void Refresh(string key)
        {
            RefreshAsync(key).Wait();
        }

        public async Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
        {
            using (var cachedEntry = Collection.AsQueryable().SingleOrDefault(i => i.Id == key))
            {
                if (cachedEntry == null)
                    return;

                cachedEntry.CreationDateTimeOffset = DateTimeOffset.UtcNow;

                if (cachedEntry.IsExpired())
                   await Collection.DeleteOneAsync(Builders<CacheEntry>.Filter.Eq("_id", key), token);
            }
        }

        public void Remove(string key)
        {
            RemoveAsync(key).Wait();
        }

        public async Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
        {
            using (var cachedEntry = Collection.AsQueryable().SingleOrDefault(i => i.Id == key))
            {
                if (cachedEntry != null)
                    await Collection.DeleteOneAsync(Builders<CacheEntry>.Filter.Eq("_id", key), token);
            }
        }

        public void Dispose()
        {
            _clearExpiredItems?.Change(Timeout.Infinite, Timeout.Infinite);
            _clearExpiredItems?.Dispose();
        }

        private void RemoveExpired(object state)
        {
            foreach (var cacheEntry in Collection.AsQueryable().ToArray())
            {
                using (cacheEntry)
                {
                    Collection.DeleteOne(Builders<CacheEntry>.Filter.Eq("_id", cacheEntry.Id));
                }
            }

            GC.Collect();
        }
    }
}