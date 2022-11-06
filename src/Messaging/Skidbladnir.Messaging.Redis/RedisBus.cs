using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Skidbladnir.Messaging.Redis
{
    internal class RedisBus
    {
        private const string EventKeyTemplate = "messaging:{0}:event:{1}";
        private const string CommandQueueTemplate = "messaging:{0}:command:{1}.queue";

        private readonly ILogger<RedisBus> _logger;
        private readonly RedisBusModuleConfiguration _configuration;
        private readonly Lazy<ConnectionMultiplexer> _redisConnectionMultiplexer;
        private readonly IList<IRedisConsumer> _commandConsumers;

        private bool _stopping = false;

        public RedisBus(ILogger<RedisBus> logger,
            RedisBusModuleConfiguration configuration
        )
        {
            _logger = logger;
            _configuration = configuration;
            _redisConnectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnection);
            _commandConsumers = new List<IRedisConsumer>();
        }

        public Task PublishAsync(Type messageType, object message)
        {
            var pubsubChannelName = string.Format(EventKeyTemplate, _configuration.VirtualHost, messageType.Name);
            var redisChannel = new RedisChannel(pubsubChannelName, RedisChannel.PatternMode.Literal);
            var subscriber = _redisConnectionMultiplexer.Value.GetSubscriber();
            return subscriber.PublishAsync(redisChannel, new RedisValue(JsonSerializer.Serialize(message)),
                CommandFlags.FireAndForget);
        }

        public async Task SendAsync(Type messageType, object message, string serviceName)
        {
            var queue = string.Format(CommandQueueTemplate, serviceName, messageType.Name);
            await PushCommand(queue, message);
        }

        public async Task Subscribe(IRedisConsumer consumer)
        {
            var eventChannelName = string.Format(EventKeyTemplate, _configuration.VirtualHost, consumer
                .GetMessageType()
                .Name);

            var eventChannel = new RedisChannel(eventChannelName, RedisChannel.PatternMode.Literal);

            var subscriber = _redisConnectionMultiplexer.Value.GetSubscriber();
            await subscriber.SubscribeAsync(eventChannel, (channel, value) => consumer.Consume(channel, value));
            _commandConsumers.Add(consumer);
        }

        public async Task StartCommandProcessing()
        {
            var commandConsumersTasks = new List<Task>();
            while (!_stopping)
            {
                
                foreach (var consumer in _commandConsumers)
                    commandConsumersTasks.Add(ProcessCommands(consumer));

                await Task.WhenAll(commandConsumersTasks);
                commandConsumersTasks.Clear();

                if(!_stopping)
                    await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        public Task StopAsync()
        {
            _stopping = true;
            return _redisConnectionMultiplexer.Value.CloseAsync();
        }

        private async Task ProcessCommands(IRedisConsumer consumer)
        {
            var db = _redisConnectionMultiplexer.Value.GetDatabase();
            var commandQueue =
                string.Format(CommandQueueTemplate, _configuration.VirtualHost, consumer.GetMessageType()
                    .Name);
            var commandQueueKey = new RedisKey(commandQueue);
            var channelName = new RedisChannel(commandQueue, RedisChannel.PatternMode.Literal);

            var undeliveredCommand = await db.ListLeftPopAsync(commandQueueKey);
            var processedMessages = 0;
            while (!undeliveredCommand.IsNullOrEmpty)
            {
                await consumer.Consume(channelName, undeliveredCommand);
                undeliveredCommand = await db.ListLeftPopAsync(commandQueueKey);
                processedMessages++;
            }

            if (processedMessages > 0)
                _logger.LogInformation("Processed {ProcessedMessages} commands in queue {Queue}", processedMessages,
                commandQueue);
        }

        private ConnectionMultiplexer CreateConnection()
        {
            return ConnectionMultiplexer.Connect(_configuration.ConnectionString);
        }

        private async Task PushCommand(string queue, object message)
        {
            var db = _redisConnectionMultiplexer.Value.GetDatabase();
            await db.ListRightPushAsync(new RedisKey(queue), new RedisValue(JsonSerializer.Serialize(message)));
        }
    }
}