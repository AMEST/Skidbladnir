using System;
using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Modules
{
    /// <summary>
    /// Base class of a runnable module from a modular system with 
    /// </summary>
    public abstract class RunnableModule : Module
    {
        /// <summary>
        /// Method for starting background work of a module
        /// </summary>
        public virtual Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method for stopping the background of a module
        /// </summary>
        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}