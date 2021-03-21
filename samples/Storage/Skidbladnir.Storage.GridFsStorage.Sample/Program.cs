using Microsoft.Extensions.Hosting;
using Skidbladnir.Modules;

namespace Skidbladnir.Storage.GridFsStorage.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSkidbladnirModules<StartupModule>();
    }
}
