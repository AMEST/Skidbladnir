using System;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Abstractions;

namespace Skidbladnir.Messaging.Redis
{
    internal class RedisMessageConsumerBuilder : IMessageConsumerBuilder
    {
        private readonly IServiceCollection _services;

        public RedisMessageConsumerBuilder(IServiceCollection services)
        {
            _services = services;

        }

        public IMessageConsumerBuilder AddConsumer<T>()
        {
            return AddConsumer(typeof(T));
        }

        public IMessageConsumerBuilder AddConsumer(Type consumerType)
        {
            _services.AddSingleton(consumerType);
            var redisConsumers = consumerType.GetRedisConsumers();
            foreach (var redisConsumerType in redisConsumers)
            {
                _services.AddSingleton(typeof(IRedisConsumer), redisConsumerType);
            }
            return this;
        }
    }
}