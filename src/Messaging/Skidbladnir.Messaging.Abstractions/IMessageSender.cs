using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Messaging.Abstractions
{
    public interface IMessageSender
    {
        Task PublishEventAsync<TEvent>(TEvent evt,
            CancellationToken token = default(CancellationToken))
            where TEvent : class, new();

        Task SendCommandAsync<TCommand>(TCommand command, string serviceName,
            CancellationToken token = default(CancellationToken))
            where TCommand : class, new();
    }

}