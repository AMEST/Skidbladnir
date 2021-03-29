using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Skidbladnir.Modules
{
    public class ModuleRunner : IHostedService
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
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var startedModules = new List<Task>();
            foreach (var module in _modules.OfType<RunnableModule>())
            {
                try
                {
                    var task = module.StartAsync(_provider, cancellationToken);
                    startedModules.Add(task);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to start module {Module}", module.GetType());
                }

            }
            return Task.WhenAll(startedModules);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var stoppingModules = new List<Task>();
            foreach (var module in _modules.OfType<RunnableModule>())
            {
                try
                {
                    var task = module.StopAsync(cancellationToken);
                    stoppingModules.Add(task);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to start module {Module}", module.GetType());
                }
            }

            return Task.WhenAll(stoppingModules);
        }
    }
}