using MongoDB.Bson.Serialization;

namespace Skidbladnir.Caching.Distributed.MongoDB
{
    internal class CacheEntryMap : BsonClassMap<CacheEntry>
    {
        public CacheEntryMap()
        {
            MapProperty(x => x.Id)
                .SetIsRequired(true);
        }
    }
}