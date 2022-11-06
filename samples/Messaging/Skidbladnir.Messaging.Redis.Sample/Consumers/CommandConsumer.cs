using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Skidbladnir.Messaging.Abstractions;
using Skidbladnir.Messaging.Redis.Sample.Entities;

namespace Skidbladnir.Messaging.Redis.Sample.Consumers
{
    public class CommandConsumer : IMessageConsumer<CommandEvent>
    {
        private readonly ILogger<CommandConsumer> _logger;

        public CommandConsumer(ILogger<CommandConsumer> logger)
        {
            _logger = logger;
        }

        public Task ConsumeAsync(CommandEvent message, CancellationToken token)
        {
            _logger.LogInformation("Received command: {Command}", message.Command);
            return Task.CompletedTask;
        }
    }
}