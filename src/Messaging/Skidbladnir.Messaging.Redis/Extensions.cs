using System;
using System.Collections.Generic;
using System.Linq;
using Skidbladnir.Messaging.Abstractions;

namespace Skidbladnir.Messaging.Redis
{
    internal static class Extensions
    {
        public static Type GetMessageType(this IRedisConsumer redisConsumer)
        {
            return redisConsumer.GetType()
                .GetInterfaces()
                .Where(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IRedisConsumer<>))
                .Select(x => x.GenericTypeArguments.FirstOrDefault())
                .FirstOrDefault();
        }

        public static IEnumerable<Type> GetRedisConsumers(this Type consumerType)
        {
            var messageTypes = consumerType
                .GetInterfaces()
                .Where(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IMessageConsumer<>))
                .Select(x => x.GenericTypeArguments[0]);

            foreach (var mType in messageTypes)
            {
                yield return typeof(RedisConsumer<,>).MakeGenericType(mType, consumerType);
            }
        }
    }
}