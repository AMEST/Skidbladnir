using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Skidbladnir.DataProtection.MongoDb
{
    internal class DbXmlKeyMap : BsonClassMap<DbXmlKey>
    {
        public DbXmlKeyMap()
        {
            AutoMap();
            MapProperty(x => x.Key).SetIsRequired(true);
            MapIdMember(x => x.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            MapCreator(x => new DbXmlKey(x.Id, x.KeyId, x.Key));
        }
    }
}