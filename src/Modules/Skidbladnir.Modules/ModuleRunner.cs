using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Skidbladnir.Modules
{
    public class ModuleRunner : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly IEnumerable<Module> _modules;
        private readonly ILogger<ModuleRunner> _logger;

        public ModuleRunner(IServiceProvider provider, IEnumerable<Module> modules, ILogger<ModuleRunner> logger)
        {
            _provider = provider;
            _modules = modules;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var startedModules = new List<Task>();
            stoppingToken.Register(StopAsync);
            foreach (var module in _modules.OfType<RunnableModule>())
            {
                try
                {
                    var task = module.StartAsync(_provider, stoppingToken);
                    startedModules.Add(task);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to start module {Module}", module.GetType());
                }

            }
            return Task.WhenAll(startedModules);
        }
        
        public void StopAsync()
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var stoppingModules = new List<Task>();
            foreach (var module in _modules.OfType<RunnableModule>())
            {
                try
                {
                    var task = module.StopAsync(tokenSource.Token);
                    stoppingModules.Add(task);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to start module {Module}", module.GetType());
                }

            }

            Task.WaitAll(stoppingModules.ToArray());
        }
    }
}