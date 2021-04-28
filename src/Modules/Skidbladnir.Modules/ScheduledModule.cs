using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;

namespace Skidbladnir.Modules
{
    public abstract class ScheduledModule : RunnableModule
    {
        private readonly CancellationTokenSource _scheduleCancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Cron rule for running scheduled module
        /// </summary>
        public abstract string CronExpression { get; }

        /// <summary>
        /// Method that is run based on Cron expression
        /// </summary>
        public abstract Task ExecuteAsync(IServiceProvider provider, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public override Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            Task.Run(async () =>
            {
                var cronExpression = Cronos.CronExpression.Parse(CronExpression);
                var nextExecutionDelay = GetDelayBeforeNextExecution(cronExpression);
                while (!_scheduleCancellationTokenSource.Token.IsCancellationRequested)
                {
                    await Task.Delay(nextExecutionDelay, _scheduleCancellationTokenSource.Token);

                    await ExecuteAsync(provider, _scheduleCancellationTokenSource.Token);

                    nextExecutionDelay = GetDelayBeforeNextExecution(cronExpression);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task StopAsync(CancellationToken cancellationToken = default)
        {
            _scheduleCancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private static TimeSpan GetDelayBeforeNextExecution(CronExpression expression)
        {
            var nextOccureTime = expression.GetNextOccurrence(DateTime.UtcNow);
            if (nextOccureTime == null)
                return TimeSpan.Zero;

            var nextExecutionTime = nextOccureTime - DateTime.UtcNow;
            return nextExecutionTime.Value >= TimeSpan.Zero
                ? nextExecutionTime.Value
                : TimeSpan.Zero;
        }
    }
}