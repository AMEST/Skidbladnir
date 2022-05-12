using System.IO;
using System.Threading.Tasks;

namespace Skidbladnir.Storage.Abstractions
{
    /// <summary>
    /// File storage abstraction
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Get all files in path
        /// </summary>
        Task<FileInfo[]> GetFilesAsync(string path);

        /// <summary>
        /// Get file info by path
        /// </summary>
        Task<FileInfo> GetFileAsync(string pathToFile);

        /// <summary>
        /// Copy file
        /// </summary>
        Task CopyAsync(string srcPath, string destPath);

        /// <summary>
        /// Move file
        /// </summary>
        Task MoveAsync(string srcPath, string destPath);

        /// <summary>
        /// Delete file
        /// </summary>
        Task DeleteAsync(string pathToFile);

        /// <summary>
        /// Check file exist
        /// </summary>
        Task<bool> Exist(string path);

        /// <summary>
        /// Upload file
        /// </summary>
        Task<FileInfo> UploadFileAsync(
            Stream stream,
            string pathToFile
        );

        /// <summary>
        /// Download file
        /// </summary>
        Task<DownloadResult> DownloadFileAsync(string pathToFile);
    }

    public interface IStorage<TStorageInfo> : IStorage where TStorageInfo : IStorageInfo
    {
    }
}