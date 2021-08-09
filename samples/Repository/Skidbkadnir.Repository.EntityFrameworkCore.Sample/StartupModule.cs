using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities;
using Skidbladnir.Modules;
using Skidbladnir.Repository.Abstractions;
using Skidbladnir.Repository.EntityFrameworkCore;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample
{
    public class StartupModule : RunnableModule
    {
        public override void Configure(IServiceCollection services)
        {
            services.RegisterContext<SampleDbContext>(
                builder => builder.UseSqlite("Data Source=sample.db;Cache=Shared"),
                entities =>
                {
                    entities.AddEntity<SimpleMessage>();
                    entities.AddEntity<SimpleGuid, SimpleGuidConfiguration>();
                }
            );
        }

        public override async Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {

            using (var scope = provider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<StartupModule>>();
                var messagesRepository = scope.ServiceProvider.GetService<IRepository<SimpleMessage>>();
                await messagesRepository.Create(new SimpleMessage() {Text = "test123", Timestamp = DateTime.UtcNow});
                await messagesRepository.Create(new SimpleMessage() {Text = "123Test", Timestamp = DateTime.UtcNow});
                await messagesRepository.Create(new SimpleMessage() {Text = "123Test321", Timestamp = DateTime.UtcNow});

                var items = await messagesRepository.GetAll().ToArrayAsync(cancellationToken);
                foreach (var item in items)
                    logger.LogInformation("{0} : Text = {1} ; DateTime = {2}", item.Id, item.Text, item.Timestamp.ToString());

                var guidRepository = scope.ServiceProvider.GetService<IRepository<SimpleGuid>>();
                await guidRepository.Create(new SimpleGuid() {Guid = Guid.NewGuid().ToString()});

                var guidItems = await messagesRepository.GetAll().ToArrayAsync(cancellationToken);
                foreach (var item in guidItems)
                    logger.LogInformation("{0} : Text = {1} ; DateTime = {2}", item.Id, item.Text, item.Timestamp.ToString());
            }
        }
    }
}