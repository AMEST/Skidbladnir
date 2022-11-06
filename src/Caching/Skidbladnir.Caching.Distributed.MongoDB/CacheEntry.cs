using System;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class CacheEntry
    {
        private readonly TimeSpan _minRefreshTime = TimeSpan.FromSeconds(1);

        public CacheEntry(string key)
        {
            Id = key;
        }

        public string Id { get; set; }

        public byte[] Value { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }

        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public DateTimeOffset CreationDateTimeOffset { get; set; }

        public bool IsExpired()
        {
            var now = DateTimeOffset.UtcNow;
            if (AbsoluteExpiration != null)
                return AbsoluteExpiration < now;

            if (AbsoluteExpirationRelativeToNow != null)
                return CreationDateTimeOffset.Add(AbsoluteExpirationRelativeToNow.Value) < now;

            if (SlidingExpiration != null && CreationDateTimeOffset.Add(SlidingExpiration.Value) > now)
            {
                CreationDateTimeOffset = DateTimeOffset.UtcNow;
                return false;
            }

            return true;
        }

        public bool IsRefreshNeeded()
        {
            if (!SlidingExpiration.HasValue)
                return false;

            if (DateTimeOffset.UtcNow - CreationDateTimeOffset <= _minRefreshTime)
                return false;

            return true;
        }
    }
}