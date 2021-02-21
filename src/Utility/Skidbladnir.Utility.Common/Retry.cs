using System;
using System.Threading.Tasks;

namespace Skidbladnir.Utility.Common
{
    public static class Retry
    {
        public static async Task<T> Do<T>(Func<Task<T>> action, int retryCount = 3, TimeSpan delay = default)
        {
            while (true)
            {
                try
                {
                    var result = await Task.Run(action);
                    return result;
                }
                catch when (retryCount-- > 0)
                {
                }

                if (delay != default)
                    await Task.Delay(delay);
            }
        }

        public static async Task Do(Func<Task> action, int retryCount = 3, TimeSpan delay = default)
        {
            while (true)
            {
                try
                {
                    await Task.Run(action);
                    return;
                }
                catch when (retryCount-- > 0)
                {
                }

                if (delay != default)
                    await Task.Delay(delay);
            }
        }
    }
}