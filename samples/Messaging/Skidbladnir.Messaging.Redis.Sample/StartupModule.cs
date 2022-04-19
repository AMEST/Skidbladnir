using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Redis.Sample.Consumers;
using Skidbladnir.Modules;

namespace Skidbladnir.Messaging.Redis.Sample
{
    public class StartupModule : RunnableModule
    {
        private IServiceProvider _provider;

        public override Type[] DependsModules => new[] {typeof(PublisherModule)};

        public override void Configure(IServiceCollection services)
        {
            var redisConfiguration = Configuration.Get<RedisBusModuleConfiguration>();
            services.AddRedisBus(redisConfiguration)
                .AddConsumer<CommandConsumer>()
                .AddConsumer<DateTimeConsumer>();
        }

        public override Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            _provider = provider;
            return provider.StartRedisBus();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return _provider.StopRedisBus();
        }
    }
}