using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Abstractions;

namespace Skidbladnir.Messaging.Redis
{
    public static class ConfigurationExtensions
    {
        public static IMessageConsumerBuilder AddRedisBus(this IServiceCollection services, RedisBusModuleConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<RedisBus>();
            services.AddSingleton<IMessageSender, RedisMessageSender>();
            var builder = new RedisMessageConsumerBuilder(services);
            return builder;
        }

        public static async Task StartRedisBus(this IServiceProvider provider)
        {
            var bus = provider.GetService<RedisBus>();
            var consumers = provider.GetService<IEnumerable<IRedisConsumer>>();
            foreach (var consumer in consumers)
            {
                await bus.ProcessUndeliveredCommands(consumer);
                await bus.Subscribe(consumer);
            }
        }

        public static async Task StopRedisBus(this IServiceProvider provider)
        {
            var bus = provider.GetService<RedisBus>();
            await bus.StopAsync();
        }
    }
}