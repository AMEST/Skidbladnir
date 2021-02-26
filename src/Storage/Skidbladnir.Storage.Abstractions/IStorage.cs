using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Skidbladnir.Storage.Abstractions
{
    public interface IStorage<TStorageInfo> where TStorageInfo : IStorageInfo
    {
        Task<FileInfo[]> GetFilesAsync(string path);

        Task<FileInfo> GetFileAsync(string pathToFile);

        Task CopyAsync(string srcPath, string destPath);

        Task MoveAsync(string srcPath, string destPath);

        Task DeleteAsync(string pathToFile);

        Task<FileInfo> UploadFileAsync(
            Stream stream,
            string pathToFile,
            IDictionary<string, string> attributes = null
        );

        Task<DownloadResult> DownloadFileAsync(string pathToFile);
    }
}