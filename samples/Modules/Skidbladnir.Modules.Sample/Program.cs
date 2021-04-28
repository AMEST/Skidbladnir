using System;
using Microsoft.Extensions.Hosting;

namespace Skidbladnir.Modules.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSkidbladnirModules<StartupModule>(configuration =>
                {
                    configuration.Add(new LongRunningModuleConfiguration(){Delay = TimeSpan.FromSeconds(2)});
                });
    }
}
