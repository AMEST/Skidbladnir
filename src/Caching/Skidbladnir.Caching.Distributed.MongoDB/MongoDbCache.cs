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
            return GetAsync(key).GetAwaiter().GetResult();
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
        {
            using (var cachedEntry = _cacheRepository.GetAll().SingleOrDefault(i => i.Id == key))
            {
                if (cachedEntry == null)
                    return null;

                if (cachedEntry.IsExpired())
                {
                    await _cacheRepository.Delete(cachedEntry);
                    return null;
                }

                await _cacheRepository.Update(cachedEntry);
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
                await _cacheRepository.Update(cachingEntry);
            }
        }

        public void Refresh(string key)
        {
            RefreshAsync(key).Wait();
        }

        public async Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
        {
            using (var cachedEntry = _cacheRepository.GetAll().SingleOrDefault(i => i.Id == key))
            {
                if (cachedEntry == null)
                    return;

                cachedEntry.CreationDateTimeOffset = DateTimeOffset.UtcNow;

                if (cachedEntry.IsExpired())
                    await _cacheRepository.Delete(cachedEntry);
            }
        }

        public void Remove(string key)
        {
            RemoveAsync(key).Wait();
        }

        public async Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
        {
            using (var cachedEntry = _cacheRepository.GetAll().SingleOrDefault(i => i.Id == key))
            {
                if (cachedEntry != null)
                    await _cacheRepository.Delete(cachedEntry);
            }
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
                    _cacheRepository.Delete(cacheEntry).Wait();
                }
            }

            GC.Collect();
        }
    }
}