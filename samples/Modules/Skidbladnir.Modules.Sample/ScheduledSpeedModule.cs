using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skidbladnir.Modules.Sample.Core;

namespace Skidbladnir.Modules.Sample
{
    public class ScheduledSpeedModule : ScheduledModule
    {
        public override string CronExpression => "* * * * *";

        public override async Task ExecuteAsync(IServiceProvider provider,
            CancellationToken cancellationToken = default)
        {
            var logger = provider.GetService<ILogger<ScheduledSpeedModule>>();
            try
            {
                const string url = "https://google.com";
                var speedTest = provider.GetService<ISpeedTest>();

                var result = await speedTest.Check(url, cancellationToken);
                logger.LogInformation("Get {Url} with speed {Speed}. Elapsed seconds {Elapsed} s", url, result.Speed,
                    result.ElapsedSeconds);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in long running module");
            }
        }
    }
}