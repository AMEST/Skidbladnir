using System;
using System.Threading;
using System.Threading.Tasks;

namespace Skidbladnir.Modules
{
    public abstract class BackgroundModule : RunnableModule
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingTokenSource = new CancellationTokenSource();

        /// <summary>
        /// The method that starts and continues in the background
        /// </summary>
        public abstract Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken = default);

        public override Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            _executingTask = ExecuteAsync(provider, _stoppingTokenSource.Token);

            if (_executingTask.IsCompleted)
                return _executingTask;

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_executingTask == null)
                return;

            try
            {
                _stoppingTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
    }
}