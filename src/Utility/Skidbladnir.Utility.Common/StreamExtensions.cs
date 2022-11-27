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
                throw new ArgumentException("Stream is not readable", nameof(input));

            if (input is MemoryStream memoryStream)
            {
                if (memoryStream.TryGetBuffer(out var buffer) 
                    && buffer.Array != null 
                    && buffer.Offset == 0 
                    && buffer.Count == memoryStream.Length)
                    return buffer.Array;

                return memoryStream.ToArray();
            }

            var inputStreamLength = TryGetStreamLength(input);
            using (memoryStream = new MemoryStream(inputStreamLength))
            {
                input.CopyTo(memoryStream);
                return inputStreamLength == 0 ? memoryStream.ToArray() : memoryStream.GetBuffer();
            }
        }

        /// <summary>
        ///     Read all bytes from stream to memory
        /// </summary>
        /// <param name="input">Stream</param>
        public static async Task<byte[]> ReadAllBytesAsync(this Stream input)
        {
            if (!input.CanRead)
                throw new ArgumentException("Stream is not readable", nameof(input));

            if (input is MemoryStream memoryStream)
            {
                if (memoryStream.TryGetBuffer(out var buffer)
                    && buffer.Array != null
                    && buffer.Offset == 0
                    && buffer.Count == memoryStream.Length)
                    return buffer.Array;

                return memoryStream.ToArray();
            }

            var inputStreamLength = TryGetStreamLength(input);
            using (memoryStream = new MemoryStream(inputStreamLength))
            {
                await input.CopyToAsync(memoryStream)
                    .ConfigureAwait(false);
                return inputStreamLength == 0 ? memoryStream.ToArray() : memoryStream.GetBuffer();
            }
        }

        private static int TryGetStreamLength(Stream input)
        {
            try
            {
                return (int) input.Length;
            }
            catch (NotSupportedException)
            {
                return 0;
            }
        }
    }

}