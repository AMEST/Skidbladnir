using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class MongoDbCache : IDistributedCache, IDisposable
    {
        private readonly IRepository<CacheEntry> _cacheRepository;
        private readonly Timer _clearExpiredItems;

        public MongoDbCache(IRepository<CacheEntry> cacheRepository)
        {
            _cacheRepository = cacheRepository;
            _clearExpiredItems =
                new Timer(
                    callback: RemoveExpired,
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromMinutes(10));
        }

        public byte[] Get(string key)
        {
            using (var cachedEntry = _cacheRepository.GetAll().Where(i => i.Id == key).SingleOrDefault())
            {
                if (cachedEntry == null)
                    return null;

                if (cachedEntry.IsExpired())
                {
                    _cacheRepository.Delete(cachedEntry);
                    return null;
                }

                _cacheRepository.Update(cachedEntry);
                return cachedEntry.Value;
            }
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
        {
            return Task.Run(() => Get(key), token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
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
                _cacheRepository.Update(cachingEntry);
            }
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = new CancellationToken())
        {
            return Task.Run(() => Set(key, value, options), token);
        }

        public void Refresh(string key)
        {
            using (var cachedEntry = _cacheRepository.GetAll().Where(i => i.Id == key).SingleOrDefault())
            {
                if (cachedEntry == null)
                    return;

                cachedEntry.CreationDateTimeOffset = DateTimeOffset.UtcNow;

                if (cachedEntry.IsExpired())
                    _cacheRepository.Delete(cachedEntry);
            }
        }

        public Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
        {
            return Task.Run(() => Refresh(key), token);
        }

        public void Remove(string key)
        {
            using (var cachedEntry = _cacheRepository.GetAll().Where(i => i.Id == key).SingleOrDefault())
            {
                if (cachedEntry != null)
                    _cacheRepository.Delete(cachedEntry);
            }
        }

        public Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
        {
            return Task.Run(() => Remove(key), token);
        }

        public void Dispose()
        {
            _clearExpiredItems?.Change(Timeout.Infinite, Timeout.Infinite);
            _clearExpiredItems?.Dispose();
        }

        private void RemoveExpired(object state)
        {
            foreach (var cacheEntry in _cacheRepository.GetAll().ToArray())
            {
                using (cacheEntry)
                {
                    _cacheRepository.Delete(cacheEntry);
                }
            }
            GC.Collect();
        }
    }
}