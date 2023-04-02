# [Skidbladnir Home](../../../README.md)

## [Storage](../README.md)

## S3 storage

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Storage.S3.svg?label=Skidbladnir.Storage.S3)](https://www.nuget.org/packages/Skidbladnir.Storage.S3/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

Implementation of file storage abstraction based on Amazon S3 protocol.

Well suited for any size applications that work with content and do not have their own persistent storage, as well as for storing files that are used by two or more instances.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Storage.S3
```

### Using

#### Register FileStorage in IoC Container

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddS3Storage(new S3StorageConfiguration(){
        ServiceUrl = "https://minio.local/",
        AccessKey = "User",
        SecretKey = "Pass",
        Bucket = "Service-Files"
        });
    service.AddHostedService<TestBackgroundService>(); //Add sample service
}
```

#### Resolve and use

```c#
public class TestBackgroundService : BackgroundService{
    private IStorage<S3StorageInfo> _storage;
    private ILogger<StartupModule> _logger;
    public TestBackgroundService(ILogger<StartupModule> logger , IStorage<S3StorageInfo> storage){
        _storage = storage;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentDir = await _storage.GetFilesAsync("");
        _logger.LogInformation("Files in root dir");
        foreach (var fileInfo in currentDir)
        {
            _logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                fileInfo.FileName,
                fileInfo.Size, fileInfo.CreatedDate);
        }
    }
}
```
