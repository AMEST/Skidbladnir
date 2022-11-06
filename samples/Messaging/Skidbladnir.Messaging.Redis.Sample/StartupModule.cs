using System;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Redis.Sample.Consumers;
using Skidbladnir.Modules;

namespace Skidbladnir.Messaging.Redis.Sample
{
    public class StartupModule : RunnableModule
    {
        public override Type[] DependsModules => new[] {typeof(PublisherModule), typeof(RedisBusModule)};

        public override void Configure(IServiceCollection services)
        {
            services.ConfigureConsumers()
                .AddConsumer<CommandConsumer>()
                .AddConsumer<DateTimeConsumer>();
        }
    }
}