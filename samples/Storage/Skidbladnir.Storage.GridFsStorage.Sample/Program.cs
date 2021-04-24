using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Skidbladnir.Modules;
using Skidbladnir.Storage.GridFS;

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
                .UseSkidbladnirModules<StartupModule>(configuration =>
                {
                    var storageConfiguration =
                        configuration.AppConfiguration.GetSection("storage").Get<GridFsStorageConfiguration>();
                    configuration.Add(storageConfiguration);
                });
    }
}
