using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddGridFsStorage("mongodb://storage:pa$$word@vm-docker-erik:27017/StorageGFTest?authSource=admin");
                });
    }
}
