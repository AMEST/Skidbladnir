using System;
using System.IO;

namespace Skidbladnir.Storage.Abstractions
{
    /// <summary>
    /// File download result
    /// </summary>
    public class DownloadResult: IDisposable
    {
        public DownloadResult(FileInfo info, Stream content)
        {
            Info = info;
            Content = content;
        }

        /// <summary>
        /// File information
        /// </summary>
        public FileInfo Info { get; }

        /// <summary>
        /// File stream
        /// </summary>
        public Stream Content { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Content?.Dispose();
        }
    }
}