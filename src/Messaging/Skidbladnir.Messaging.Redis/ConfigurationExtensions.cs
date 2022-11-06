using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Messaging.Abstractions;

namespace Skidbladnir.Messaging.Redis
{
    public static class ConfigurationExtensions
    {
        public static IMessageConsumerBuilder AddRedisBus(this IServiceCollection services, RedisBusModuleConfiguration configuration)
        {
            var module = new RedisBusModule();
            module.Configure(services);
            services.AddSingleton(configuration);
            var builder = new RedisMessageConsumerBuilder(services);
            return builder;
        }

        public static IMessageConsumerBuilder ConfigureConsumers(this IServiceCollection services)
        {
            return new RedisMessageConsumerBuilder(services);
        }

        public static Task StartRedisBus(this IServiceProvider provider)
        {
            var busService = provider.GetService<RedisBusModule>();
            return busService.StartAsync(provider, CancellationToken.None);
        }

        public static Task StopRedisBus(this IServiceProvider provider)
        {
            var busService = provider.GetService<RedisBusModule>();
            return busService.StopAsync(CancellationToken.None);
        }
    }
}