using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Skidbladnir.Modules
{
    /// <summary>
    /// Base class of a module from a modular system
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// List of dependent modules that will be initialized and launched (if it is a runnable module)
        /// </summary>
        public virtual Type[] DependsModules { get; }

        /// <summary>
        /// Configurations from Microsoft Extensions Configuration
        /// </summary>
        public IConfiguration Configuration { get; internal set; }

        /// <summary>
        /// Configure IoC Container
        /// </summary>
        public virtual void Configure(IServiceCollection services)
        {
        }
    }
}