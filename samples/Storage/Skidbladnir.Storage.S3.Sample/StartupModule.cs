using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skidbladnir.Modules;
using Skidbladnir.Storage.Abstractions;
using FileInfo = Skidbladnir.Storage.Abstractions.FileInfo;
namespace Skidbladnir.Storage.S3.Sample
{
    public class StartupModule : RunnableModule
    {
        private ILogger<StartupModule> _logger;
        private IStorage<S3StorageInfo> _storage;

        public override void Configure(IServiceCollection services)
        {
            var storageConfiguration = Configuration.Get<S3StorageConfiguration>();
            services.AddS3Storage(storageConfiguration);
        }

        public override async Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            _logger = provider.GetService<ILogger<StartupModule>>();
            _storage = provider.GetService<IStorage<S3StorageInfo>>();
            try
            {
                var testString = $"This is a test text for write to file {DateTime.UtcNow}";
                var testBinary = Encoding.UTF8.GetBytes(testString);

                var currentDir = await _storage.GetFilesAsync("");
                _logger.LogInformation("Files in root dir");
                foreach (var fileInfo in currentDir)
                {
                    _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                        fileInfo.FileName,
                        fileInfo.Size, fileInfo.CreatedDate);
                    await DownloadFileAndPrint(fileInfo);
                }

                _logger.LogInformation("upload file:");
                var testFileUploadStream = new MemoryStream(testBinary);
                var uploadFileinfo = await _storage.UploadFileAsync(testFileUploadStream, "testFile.txt");
                _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                    uploadFileinfo.FileName,
                    uploadFileinfo.Size, uploadFileinfo.CreatedDate);

                _logger.LogInformation("copy file");
                await _storage.CopyAsync(uploadFileinfo.FilePath, $"{uploadFileinfo.FilePath}.new");
                await _storage.CopyAsync(uploadFileinfo.FilePath,
                    $"newFolder{Path.DirectorySeparatorChar}{uploadFileinfo.FilePath}.new");

                _logger.LogInformation("move file");
                await _storage.MoveAsync($"{uploadFileinfo.FilePath}.new", $"{uploadFileinfo.FilePath}.backup");
                await _storage.CopyAsync($"{uploadFileinfo.FilePath}.backup", $"very/long/sub/dir/path/{uploadFileinfo.FilePath}");

                _logger.LogInformation("remove base file");
                await _storage.DeleteAsync(uploadFileinfo.FilePath);

                _logger.LogInformation("Files in root dir");
                currentDir = await _storage.GetFilesAsync("");
                foreach (var fileInfo in currentDir)
                {
                    _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                        fileInfo.FileName,
                        fileInfo.Size, fileInfo.CreatedDate);
                    await DownloadFileAndPrint(fileInfo);
                }

                _logger.LogInformation("Files in newFolder dir");
                currentDir = await _storage.GetFilesAsync("newFolder");
                foreach (var fileInfo in currentDir)
                {
                    _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                        fileInfo.FileName,
                        fileInfo.Size, fileInfo.CreatedDate);
                    await DownloadFileAndPrint(fileInfo);
                }

                _logger.LogInformation("Files in very/long/sub/dir/path/ dir");
                currentDir = await _storage.GetFilesAsync("very/long/sub/dir/path/");
                foreach (var fileInfo in currentDir)
                {
                    _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                        fileInfo.FileName,
                        fileInfo.Size, fileInfo.CreatedDate);
                    await DownloadFileAndPrint(fileInfo);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in storage sample");
            }
        }

        private async Task DownloadFileAndPrint(FileInfo info)
        {
            var result = await _storage.DownloadFileAsync(info.FilePath);
            await using var fileStream = result.Content;
            using var streamReader = new StreamReader(fileStream);
            var fileContent = await streamReader.ReadToEndAsync();
            _logger.LogInformation("File Content: {Content}", fileContent);
        }
    }
}