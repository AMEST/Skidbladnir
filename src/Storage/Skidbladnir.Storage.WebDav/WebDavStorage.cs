using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Skidbladnir.Storage.Abstractions;
using WebDav;

namespace Skidbladnir.Storage.WebDav
{
    public class WebDavStorage<TStorageInfo> : IStorage<TStorageInfo> where TStorageInfo : WebDavStorageInfo
    {
        private readonly TStorageInfo _storageInfo;

        public WebDavStorage(TStorageInfo storageInfo)
        {
            _storageInfo = storageInfo;
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri(storageInfo.Configuration.Address),
                Credentials = new NetworkCredential(
                    storageInfo.Configuration.Username,
                    storageInfo.Configuration.Password
                )
            };

            Client = new WebDavClient(clientParams);

        }

        protected WebDavClient Client { get; }

        public async Task CopyAsync(string srcPath, string destPath)
        {
            await MakeDirStructure(destPath.GetPathWithoutFileName());
            await Client.Copy(srcPath, destPath);
        }

        public async Task DeleteAsync(string pathToFile)
        {
            await MakeDirStructure(pathToFile.GetPathWithoutFileName());
            await Client.Delete(pathToFile);
        }

        public async Task<DownloadResult> DownloadFileAsync(string pathToFile)
        {
            var fileInfo = await GetFileAsync(pathToFile);
            var fileContent = await Client.GetRawFile(pathToFile);
            return new DownloadResult(fileInfo, fileContent.Stream);
        }

        public async Task<bool> Exist(string path)
        {
            return (await GetFileAsync(path)) != null;
        }

        public async Task<Abstractions.FileInfo> GetFileAsync(string pathToFile)
        {
            var directoryFiles = await Client.Propfind(pathToFile);
            var fileInfo = directoryFiles.Resources.FirstOrDefault();
            return fileInfo?.ToFileInfo(pathToFile);
        }

        public async Task<Abstractions.FileInfo[]> GetFilesAsync(string path)
        {
            var directoryFiles = await Client.Propfind(path);
            return directoryFiles?.Resources?
                .Where(x => !x.IsCollection)
                .Select(x => x.ToFileInfo(Path.Combine(path, Path.GetFileName(x.Uri))))
                .ToArray();
        }

        public async Task MoveAsync(string srcPath, string destPath)
        {
            await MakeDirStructure(destPath.GetPathWithoutFileName());
            await Client.Move(srcPath, destPath);
        }

        public async Task<Abstractions.FileInfo> UploadFileAsync(Stream stream, string pathToFile)
        {
            await MakeDirStructure(pathToFile.GetPathWithoutFileName());
            await Client.PutFile(pathToFile, stream);
            return await GetFileAsync(pathToFile);
        }

        private async Task MakeDirStructure(string destDir)
        {
            var allSubDirectories = destDir.Split('/');
            var subDirectory = string.Empty;
            foreach (var dir in allSubDirectories)
            {
                subDirectory += string.IsNullOrEmpty(subDirectory)
                    ? dir
                    : $"/{dir}";
                await Client.Mkcol(subDirectory);
            }
        }
    }
}