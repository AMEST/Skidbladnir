using System;
using System.Threading.Tasks;

namespace Skidbladnir.Utility.Common
{
    public static class Try
    {
        /// <summary>
        ///     The method allows you to perform an operation by handling a possible error
        /// </summary>
        /// <param name="func">Function</param>
        /// <param name="isAllowedToCatch">Checks if the given error can be handled</param>
        public static async Task DoAsync(Func<Task> func, Func<Exception, bool> isAllowedToCatch = null)
        {
            await DoAsync<object>(async () =>
            {
                await func();
                return default;
            }, isAllowedToCatch);
        }

        /// <summary>
        ///     The method allows you to perform an operation and return a value, or default in case of an error
        /// </summary>
        /// <param name="func">Function</param>
        /// <param name="isAllowedToCatch">Checks if the given error can be handled</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(Func<Task<T>> func, Func<Exception, bool> isAllowedToCatch = null)
        {
            try
            {
                return await func();
            }
            catch (Exception e)
            {
                if (isAllowedToCatch != null && !isAllowedToCatch(e))
                    throw;

                return default;
            }
        }
    }
}