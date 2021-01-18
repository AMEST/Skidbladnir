using Skidbladnir.Repository.MongoDB;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class CacheEntryMap : EntityMapClass<CacheEntry>
    {
        public CacheEntryMap()
        {
            ToCollection("DistributedCache");
            MapProperty(x => x.Id)
                .SetIsRequired(true);
        }
    }
}