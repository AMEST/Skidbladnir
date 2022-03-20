using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Skidbladnir.Messaging.Redis
{
    internal class RedisBus
    {
        private const string EventKeyTemplate = "messaging:{0}:event:{1}";
        private const string CommandKeyTemplate = "messaging:{0}:command:{1}:{2}";
        private const string CommandQueueTemplate = "messaging:{0}:command:{1}.queue";

        private readonly ILogger<RedisBus> _logger;
        private readonly RedisBusModuleConfiguration _configuration;
        private readonly Lazy<ConnectionMultiplexer> _redisConnectionMultiplexer;

        public RedisBus(ILogger<RedisBus> logger, RedisBusModuleConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _redisConnectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnection);
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
            var searchPattern = string.Format(CommandKeyTemplate, serviceName, messageType.Name, "*");
            var activeRedisChannels = await GetActiveRedisChannel(searchPattern);

            var randomChannel = activeRedisChannels.OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();

            if (randomChannel == null)
            {
                var queue = string.Format(CommandQueueTemplate, serviceName, messageType.Name);
                await StoreUndeliverableCommands(queue, message);
                return;
            }

            var redisChannel = new RedisChannel(randomChannel, RedisChannel.PatternMode.Literal);
            var subscriber = _redisConnectionMultiplexer.Value.GetSubscriber();
            await subscriber.PublishAsync(redisChannel, new RedisValue(JsonSerializer.Serialize(message)));
        }

        public async Task Subscribe(IRedisConsumer consumer)
        {
            var eventChannelName = string.Format(EventKeyTemplate, _configuration.VirtualHost, consumer
                .GetMessageType()
                .Name);
            var commandChannelName = string.Format(CommandKeyTemplate, _configuration.VirtualHost, consumer
                .GetMessageType()
                .Name, Environment.MachineName);

            var eventChannel = new RedisChannel(eventChannelName, RedisChannel.PatternMode.Literal);
            var commandChannel = new RedisChannel(commandChannelName, RedisChannel.PatternMode.Literal);

            var subscriber = _redisConnectionMultiplexer.Value.GetSubscriber();
            await subscriber.SubscribeAsync(eventChannel, (channel, value) => consumer.Consume(channel, value));
            await subscriber.SubscribeAsync(commandChannel, (channel, value) => consumer.Consume(channel, value));
        }

        public async Task ProcessUndeliveredCommands(IRedisConsumer consumer)
        {
            var db = _redisConnectionMultiplexer.Value.GetDatabase();
            var undeliveredQueue =
                string.Format(CommandQueueTemplate, _configuration.VirtualHost, consumer.GetMessageType()
                    .Name);
            var undeliveredQueueKey = new RedisKey(undeliveredQueue);
            var channelName = new RedisChannel(undeliveredQueue, RedisChannel.PatternMode.Literal);

            var undeliveredCommand = await db.ListLeftPopAsync(undeliveredQueueKey);
            var processedMessages = 0;
            while (!undeliveredCommand.IsNullOrEmpty)
            {
                await consumer.Consume(channelName, undeliveredCommand);
                undeliveredCommand = await db.ListLeftPopAsync(undeliveredQueueKey);
                processedMessages++;
            }

            if(processedMessages > 0)
                _logger.LogInformation("Processed {ProcessedMessages} undelivered commands in queue {Queue}", processedMessages,
                undeliveredQueue);
        }

        public Task StopAsync()
        {
            return _redisConnectionMultiplexer.Value.CloseAsync();
        }

        private ConnectionMultiplexer CreateConnection()
        {
            return ConnectionMultiplexer.Connect(_configuration.ConnectionString);
        }

        private async Task<string[]> GetActiveRedisChannel(string searchPattern)
        {
            var channelSearchPattern = new RedisChannel(searchPattern, RedisChannel.PatternMode.Auto);
            var channels = new List<RedisChannel>();
            var endpoints = _redisConnectionMultiplexer.Value.GetEndPoints();
            foreach (var endPoint in endpoints)
            {
                var server = _redisConnectionMultiplexer.Value.GetServer(endPoint);
                channels.AddRange(await server.SubscriptionChannelsAsync(channelSearchPattern));
            }

            return channels.Select(x => x.ToString())
                .Distinct()
                .ToArray();
        }

        private async Task StoreUndeliverableCommands(string queue, object message)
        {
            _logger.LogWarning("No active consumers on channel. Store message to queue {Queue}", queue);
            var db = _redisConnectionMultiplexer.Value.GetDatabase();
            await db.ListRightPushAsync(new RedisKey(queue), new RedisValue(JsonSerializer.Serialize(message)));
        }
    }
}