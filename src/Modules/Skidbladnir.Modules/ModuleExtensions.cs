using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: InternalsVisibleTo("Skidbladnir.Modules.Tests")]

namespace Skidbladnir.Modules
{
    /// <summary>
    /// Skidbladnir Modular system management extensions
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Integration of a modular system with Microsoft.Extensions.Hosting.
        /// Register dependencies from module, add IConfiguration, register ModuleRunner HostedService
        /// </summary>
        public static IHostBuilder UseSkidbladnirModules<TModule>(this IHostBuilder hostBuilder)
            where TModule : Module, new()
        {
            hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddSkidbladnirModules<TModule>(context.Configuration);
            });
            return hostBuilder;
        }

        /// <summary>
        /// Integration of a modular system with Microsoft Extensions Dependencies Injecton
        /// Register dependencies from module, add IConfiguration, register ModuleRunner HostedService
        /// </summary>
        public static IServiceCollection AddSkidbladnirModules<TModule>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration = null
        )
            where TModule : Module, new()
        {
            if (configuration == null)
                configuration = ConfigureDefaultConfiguration();

            var modules = GetModulesRecursively(typeof(TModule));
            foreach (var module in modules)
            {
                module.Configuration = configuration;
                module.Configure(serviceCollection);
            }

            serviceCollection.AddHostedService<ModuleRunner>();
            serviceCollection.AddSingleton<IEnumerable<Module>>(modules.AsEnumerable());
            return serviceCollection;
        }

        /// <summary>
        /// Manual start module runner if don't using IHosting
        /// </summary>
        public static Task StartModules(this IServiceProvider provider)
        {
            var moduleRunner = GetModuleRunner(provider);
            if (moduleRunner == null)
                throw new ModuleInitializeException(
                    $"Can't start modules because {typeof(ModuleRunner).Name} not registered");
            return moduleRunner.StartAsync(CancellationToken.None);
        }

        /// <summary>
        /// Manual stop module runner if don't using IHosting
        /// </summary>
        public static Task StopModules(this IServiceProvider provider)
        {
            var moduleRunner = GetModuleRunner(provider);
            if (moduleRunner == null)
                throw new ModuleInitializeException(
                    $"Can't start modules because {typeof(ModuleRunner).Name} not registered");
            return moduleRunner.StopAsync(CancellationToken.None);
        }

        internal static Module[] GetModulesRecursively(Type moduleType)
        {
            var modules = new List<Module>();
            if (!typeof(Module).IsAssignableFrom(moduleType))
                throw new ModuleInitializeException(
                    $"Тип {moduleType.Name} не является наследником {typeof(Module).Name}");

            var module = (Module) Activator.CreateInstance(moduleType);
            modules.Add(module);
            if (module.DependsModules == null)
                return modules.ToArray();

            foreach (var dependedModule in module.DependsModules)
            {
                var rModules = GetModulesRecursively(dependedModule);
                modules.AddRange(rModules.Where(rm => modules.All(am => rm.GetType() != am.GetType())));
            }

            return modules.ToArray();
        }

        private static IHostedService GetModuleRunner(IServiceProvider provider)
        {
            var hostedServices = provider.GetServices<IHostedService>();
            var moduleRunner = hostedServices.FirstOrDefault(h => typeof(ModuleRunner) == h.GetType());
            return moduleRunner;
        }

        private static IConfiguration ConfigureDefaultConfiguration()
        {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            config.AddEnvironmentVariables();

            return config.Build();
        }
    }
}