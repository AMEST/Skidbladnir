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
namespace Skidbladnir.Storage.LocalFileStorage.Sample
{
    public class StartupModule : RunnableModule
    {
        private ILogger<StartupModule> _logger;
        private IStorage<LocalStorageInfo> _localStorage;

        public override void Configure(IServiceCollection services)
        {
            services.AddLocalFsStorage(Configuration["Storage:Path"]);
        }

        public override async Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            _logger = provider.GetService<ILogger<StartupModule>>();
            _localStorage = provider.GetService<IStorage<LocalStorageInfo>>();
            try
            {
                var testString = $"This is a test text for write to file {DateTime.UtcNow}";
                var testBinary = Encoding.UTF8.GetBytes(testString);

                var currentDir = await _localStorage.GetFilesAsync("");
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
                var uploadFileinfo = await _localStorage.UploadFileAsync(testFileUploadStream, "testFile.txt");
                _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                    uploadFileinfo.FileName,
                    uploadFileinfo.Size, uploadFileinfo.CreatedDate);

                _logger.LogInformation("copy file");
                await _localStorage.CopyAsync(uploadFileinfo.FilePath, $"{uploadFileinfo.FilePath}.new");
                await _localStorage.CopyAsync(uploadFileinfo.FilePath,
                    $"newFolder{Path.DirectorySeparatorChar}{uploadFileinfo.FilePath}.new");

                _logger.LogInformation("move file");
                await _localStorage.MoveAsync($"{uploadFileinfo.FilePath}.new", $"{uploadFileinfo.FilePath}.backup");

                _logger.LogInformation("remove base file");
                await _localStorage.DeleteAsync(uploadFileinfo.FilePath);

                _logger.LogInformation("Files in root dir");
                currentDir = await _localStorage.GetFilesAsync("");
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
            var result = await _localStorage.DownloadFileAsync(info.FilePath);
            await using var fileStream = result.Content;
            using var streamReader = new StreamReader(fileStream);
            var fileContent = await streamReader.ReadToEndAsync();
            _logger.LogInformation("File Content: {Content}", fileContent);
        }
    }
}