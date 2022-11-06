using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Skidbladnir.Messaging.Abstractions;
using Skidbladnir.Messaging.Redis.Sample.Entities;

namespace Skidbladnir.Messaging.Redis.Sample.Consumers
{
    public class DateTimeConsumer : IMessageConsumer<DateTimeEvent>
    {
        private readonly ILogger<DateTimeConsumer> _logger;

        public DateTimeConsumer(ILogger<DateTimeConsumer> logger)
        {
            _logger = logger;
        }

        public Task ConsumeAsync(DateTimeEvent message, CancellationToken token)
        {
            _logger.LogInformation("{Random} count nothing at moment {Date}", message.Random, message.Date);
            return Task.CompletedTask;
        }
    }
}