using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Abstractions;
using Skidbladnir.Modules;

namespace Skidbladnir.Messaging.Redis
{
    public class RedisBusModule : RunnableModule
    {
        private RedisBus _bus;

        public override void Configure(IServiceCollection services)
        {
            var configuration = Configuration?.Get<RedisBusModuleConfiguration>(null);
            if( configuration != null )
                services.AddSingleton(configuration);

            services.AddSingleton<RedisBus>();
            services.AddSingleton<IMessageSender, RedisMessageSender>();
            services.AddSingleton<RedisBusModule>(this);
        }

        public override async Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            _bus = provider.GetService<RedisBus>();
            var consumers = provider.GetService<IEnumerable<IRedisConsumer>>();
            foreach (var consumer in consumers)
            {
                await _bus.ProcessUndeliveredCommands(consumer);
                await _bus.Subscribe(consumer);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.StopAsync();
        }
    }
}