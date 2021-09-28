using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skidbladnir.Modules;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            MigrateDatabase(host);
            return host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSkidbladnirModules<StartupModule>()
                .UseConsoleLifetime();

        private static void MigrateDatabase(IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SampleDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
