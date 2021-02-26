using System.IO;

namespace Skidbladnir.Storage.Abstractions
{
    public class DownloadResult
    {
        public DownloadResult(FileInfo info, Stream content)
        {
            Info = info;
            Content = content;
        }

        public FileInfo Info { get; }

        public Stream Content { get; }
    }
}