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
    }
}