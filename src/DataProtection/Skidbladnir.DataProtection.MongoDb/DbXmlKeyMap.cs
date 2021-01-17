using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Skidbladnir.DataProtection.Abstractions;
using Skidbladnir.Repository.MongoDB;

namespace Skidbladnir.DataProtection.MongoDb
{
    internal class DbXmlKeyMap : EntityMapClass<DbXmlKey>
    {
        public DbXmlKeyMap()
        {
            ToCollection("dataProtectionKeys");

            MapProperty(x => x.Key).SetIsRequired(true);

            MapIdMember(x => x.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));


            MapCreator(x => new DbXmlKey(x.Id, x.KeyId, x.Key));
        }
    }

}