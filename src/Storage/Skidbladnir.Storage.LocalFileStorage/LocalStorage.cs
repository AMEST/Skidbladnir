﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Skidbladnir.Storage.Abstractions;
using FileInfo = Skidbladnir.Storage.Abstractions.FileInfo;

namespace Skidbladnir.Storage.LocalFileStorage
{
    internal class LocalStorage<TStorageInfo> : IStorage<TStorageInfo> where TStorageInfo : LocalStorageInfo
    {
        private readonly TStorageInfo _storageInfo;

        public LocalStorage(TStorageInfo storageInfo)
        {
            _storageInfo = storageInfo;
            if (!Directory.Exists(_storageInfo.StoragePath))
                Directory.CreateDirectory(_storageInfo.StoragePath);
        }

        public Task<FileInfo[]> GetFilesAsync(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path), "Can't be null");

            var fullPath = Path.Combine(_storageInfo.StoragePath, path.EscapePath());
            if (!Directory.Exists(fullPath))
                return Task.FromResult(new FileInfo[0]);

            var fileNames = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories);
            var fileInfoList = new List<FileInfo>();
            foreach (var fileName in fileNames)
            {
                var localFileInfo = new System.IO.FileInfo(fileName);
                fileInfoList.Add(localFileInfo.ToFileInfo(_storageInfo));
            }

            return Task.FromResult(fileInfoList.ToArray());
        }

        public Task<FileInfo> GetFileAsync(string pathToFile)
        {
            if (string.IsNullOrWhiteSpace(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile), "Can't be null or empty");

            var fullPath = Path.Combine(_storageInfo.StoragePath, pathToFile.EscapePath());
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            var file = new System.IO.FileInfo(fullPath);
            var fileInfo = file.ToFileInfo(_storageInfo);
            return Task.FromResult(fileInfo);
        }

        public Task CopyAsync(string srcPath, string destPath)
        {
            if (string.IsNullOrWhiteSpace(srcPath))
                throw new ArgumentNullException(nameof(srcPath), "Can't be null or empty");

            if (string.IsNullOrWhiteSpace(destPath))
                throw new ArgumentNullException(nameof(destPath), "Can't be null or empty");

            var srcFullPath = Path.Combine(_storageInfo.StoragePath, srcPath.EscapePath());
            var destFullPath = Path.Combine(_storageInfo.StoragePath, destPath.EscapePath());
            if (!File.Exists(srcFullPath))
                throw new FileNotFoundException("File not found", srcFullPath);

            if (!Directory.Exists(destFullPath.GetPathWithoutFileName()))
                Directory.CreateDirectory(destFullPath.GetPathWithoutFileName());

            File.Copy(srcFullPath, destFullPath, true);

            return Task.CompletedTask;
        }

        public Task MoveAsync(string srcPath, string destPath)
        {
            if (string.IsNullOrWhiteSpace(srcPath))
                throw new ArgumentNullException(nameof(srcPath), "Can't be null or empty");

            if (string.IsNullOrWhiteSpace(destPath))
                throw new ArgumentNullException(nameof(destPath), "Can't be null or empty");

            var srcFullPath = Path.Combine(_storageInfo.StoragePath, srcPath.EscapePath());
            var destFullPath = Path.Combine(_storageInfo.StoragePath, destPath.EscapePath());
            if (!File.Exists(srcFullPath))
                throw new FileNotFoundException("File not found", srcFullPath);

            if (!Directory.Exists(destFullPath.GetPathWithoutFileName()))
                Directory.CreateDirectory(destFullPath.GetPathWithoutFileName());

            if (File.Exists(destFullPath))
                File.Delete(destFullPath);

            File.Move(srcFullPath, destFullPath);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(string pathToFile)
        {
            if (string.IsNullOrWhiteSpace(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile), "Can't be null or empty");

            var fullPath = Path.Combine(_storageInfo.StoragePath, pathToFile.EscapePath());
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public Task<bool> Exist(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "Can't be null or empty");

            var fullPath = Path.Combine(_storageInfo.StoragePath, path.EscapePath());

            return Task.FromResult(File.Exists(fullPath));
        }

        public async Task<FileInfo> UploadFileAsync(Stream stream, string pathToFile)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "Can't be null");

            if (string.IsNullOrWhiteSpace(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile), "Can't be null or empty");

            var fullPath = Path.Combine(_storageInfo.StoragePath, pathToFile.EscapePath());

            if (!Directory.Exists(fullPath.GetPathWithoutFileName()))
                Directory.CreateDirectory(fullPath.GetPathWithoutFileName());

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            using (var fileStream = File.Create(fullPath))
            {
                await stream.CopyToAsync(fileStream);
            }

            return new FileInfo(pathToFile.EscapePath().StripPath(), stream.Length, DateTime.UtcNow);
        }

        public Task<DownloadResult> DownloadFileAsync(string pathToFile)
        {
            if (string.IsNullOrWhiteSpace(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile), "Can't be null or empty");

            var fullPath = Path.Combine(_storageInfo.StoragePath, pathToFile.EscapePath());

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            var localFileInfo = new System.IO.FileInfo(fullPath);
            var fileInfo =localFileInfo.ToFileInfo(_storageInfo);
            return Task.FromResult(new DownloadResult(fileInfo, localFileInfo.OpenRead()));
        }
    }
}