using System.Threading;
using System.Threading.Tasks;
using Skidbladnir.Messaging.Abstractions;

namespace Skidbladnir.Messaging.Redis
{
    internal class RedisMessageSender : IMessageSender
    {
        private readonly RedisBus _redisBus;

        public RedisMessageSender(RedisBus redisBus)
        {
            _redisBus = redisBus;
        }

        public Task PublishEventAsync<TEvent>(TEvent evt, CancellationToken token = default) where TEvent : class, new()
        {
            var messageType = evt.GetType();
            return _redisBus.PublishAsync(messageType, evt);
        }

        public Task SendCommandAsync<TCommand>(TCommand command, string serviceName,
            CancellationToken token = default) where TCommand : class, new()
        {
            var messageType = command.GetType();
            return _redisBus.SendAsync(messageType, command, serviceName);
        }
    }
}