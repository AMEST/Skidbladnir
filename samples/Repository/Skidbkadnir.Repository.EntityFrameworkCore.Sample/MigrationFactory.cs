using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Repository.EntityFrameworkCore;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample
{
    public class MigrationFactory : MigrationDbContextFactory<SampleDbContext>
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var startupModule = new StartupModule();
            startupModule.Configure(serviceCollection);
        }
    }
}