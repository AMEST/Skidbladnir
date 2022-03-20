using System.Threading.Tasks;
using Skidbladnir.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading;
using System;

namespace Skidbladnir.Messaging.Redis
{
    internal class RedisConsumer<TMessage, TConsumer> : IRedisConsumer<TMessage>
        where TMessage : class, new()
        where TConsumer : class, IMessageConsumer<TMessage>
    {
        private readonly TConsumer _consumer;
        private readonly ILogger<RedisConsumer<TMessage, TConsumer>> _logger;
        private readonly RedisBusModuleConfiguration _configuration;
        private readonly RedisBus _redisBus;

        public RedisConsumer(TConsumer consumer, ILogger<RedisConsumer<TMessage, TConsumer>> logger,
            RedisBusModuleConfiguration configuration,
            RedisBus redisBus)
        {
            _consumer = consumer;
            _logger = logger;
            _configuration = configuration;
            _redisBus = redisBus;
        }

        public async Task Consume(RedisChannel channel, RedisValue message)
        {
            try
            {
                var payload = JsonSerializer.Deserialize<TMessage>(message.ToString());
                await _consumer.ConsumeAsync(payload, CancellationToken.None);
            }
            catch (Exception e)
            {
                if (channel.ToString()
                    .StartsWith($"messaging:{_configuration.VirtualHost}:event"))
                {
                    _logger.LogError("Error occurred while consume message", e);
                    throw;
                }
                _logger.LogError("Error occurred while consume command. Retry send command", e);
                await _redisBus.SendAsync(typeof(TMessage), message.ToString(), _configuration.VirtualHost);
            }
        }
    }
}