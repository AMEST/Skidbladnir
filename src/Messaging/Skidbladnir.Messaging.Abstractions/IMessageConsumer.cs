using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Messaging.Abstractions
{
    public interface IMessageConsumer<in TMessage> where TMessage : class, new()
    {
        Task ConsumeAsync(TMessage message, CancellationToken token);
    }
}
