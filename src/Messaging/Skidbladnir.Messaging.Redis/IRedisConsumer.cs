using System.Threading.Tasks;
using StackExchange.Redis;

namespace Skidbladnir.Messaging.Redis
{
    internal interface IRedisConsumer
    {
        Task Consume(RedisChannel channel, RedisValue message);
    }

    internal interface IRedisConsumer<TMessage> : IRedisConsumer
        where TMessage : class, new()
    {

    }
}