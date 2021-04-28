using Microsoft.Extensions.DependencyInjection;

namespace Skidbladnir.Modules.Sample.Core
{
    public class CoreSampleModule : Module
    {
        public override void Configure(IServiceCollection services)
        {
            services.AddSingleton<ISpeedTest, SpeedTest>();
        }
    }
}