using System;
using System.IO;
using System.Threading.Tasks;

namespace Skidbladnir.Utility.Common
{
    /// <summary>
    ///     Extensions methods for Streams
    /// </summary>
    public static class StreamExtentions
    {
        /// <summary>
        ///     Read all bytes from stream to memory
        /// </summary>
        /// <param name="input">Stream</param>
        public static byte[] ReadAllBytes(this Stream input)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("Stream is not readable", nameof(input));
            }

            if (input is MemoryStream memoryStream)
                return memoryStream.ToArray();

            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Read all bytes from stream to memory
        /// </summary>
        /// <param name="input">Stream</param>
        public static async Task<byte[]> ReadAllBytesAsync(this Stream input)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("Stream is not readable", nameof(input));
            }

            if (input is MemoryStream memoryStream)
                return memoryStream.ToArray();

            using (memoryStream = new MemoryStream())
            {
                await input.CopyToAsync(memoryStream)
                    .ConfigureAwait(false);
                return memoryStream.ToArray();
            }
        }
    }

}