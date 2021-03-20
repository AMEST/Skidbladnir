using System;
using Skidbladnir.Repository.Abstractions;
using Skidbladnir.Utility.Common;

namespace Skidbladnir.Repository.MongoDB
{
    public static class Utilities
    {
        /// <summary>
        /// Create default entity configuration
        /// </summary>
        public static EntityMapClass<TEntity> CreateDefaultMongoMap<TEntity>()
            where TEntity : class, IHasId
        {
            var type = typeof(TEntity);
            var classMapDefinition = typeof(EntityMapClass<>);
            var classMapType = classMapDefinition.MakeGenericType(type);
            var classMap = (EntityMapClass<TEntity>)Activator.CreateInstance(classMapType);
            classMap.ToCollection(type.Name.Plural());
            classMap.MapId(x => x.Id);
            return classMap;
        }
    }
}