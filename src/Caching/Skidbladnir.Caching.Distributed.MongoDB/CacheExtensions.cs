using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    public static class CacheExtensions
    {
        public static bool TryGetValue<TResult>(this IDistributedCache cache, string key, out TResult result)
        {
            var cacheValue = cache.Get(key);
            if (cacheValue == null)
            {
                result = default;
                return false;
            }

            result = JsonConvert.DeserializeObject<TResult>(Encoding.UTF8.GetString(cacheValue));
            cacheValue = null;
            return true;
        }

        public static void Set(this IDistributedCache cache, string key, object entry,
            DistributedCacheEntryOptions options)
        {
            var serializedEntry = JsonConvert.SerializeObject(entry);
            cache.Set(key, Encoding.UTF8.GetBytes(serializedEntry),options);
            serializedEntry = null;
        }
    }
}