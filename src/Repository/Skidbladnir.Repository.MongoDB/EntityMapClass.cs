using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Skidbladnir.Repository.MongoDB
{

    public class EntityMapClass<TClass> : BsonClassMap<TClass>
    {

        public EntityMapClass()
        {
            AutoMap();
        }

        public string CollectionName { get; private set; }

        public EntityMapClass<TClass> ToCollection(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName)) throw new ArgumentNullException(nameof(collectionName));
            CollectionName = collectionName;
            return this;
        }

        public void MapId<TMember>(Expression<Func<TClass, TMember>> memberLambda, BsonType storeType = BsonType.ObjectId)
        {
            MapIdMember(memberLambda)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(storeType));
        }

    }
}