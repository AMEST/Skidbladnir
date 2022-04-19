using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Abstractions;
using Skidbladnir.Messaging.Redis.Sample.Entities;
using Skidbladnir.Modules;

namespace Skidbladnir.Messaging.Redis.Sample
{
    public class PublisherModule : BackgroundModule
    {
        private readonly Random _rand;

        public PublisherModule()
        {
            _rand = new Random();
        }

        public override async Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var messageSender = provider.GetService<IMessageSender>();
                await messageSender.PublishEventAsync(new DateTimeEvent()
                {
                    Date = DateTime.UtcNow,
                    Random = _rand.NextDouble()
                }, cancellationToken);

                await messageSender.SendCommandAsync(new CommandEvent()
                {
                    Command = $"Execute command eval({Guid.NewGuid()}"
                }, "testapp", cancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}