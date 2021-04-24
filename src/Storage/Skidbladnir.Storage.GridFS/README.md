# [Skidbladnir Home](../../../README.md)
## [Storage](../README.md)
## GridFS storage

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Storage.GridFs.svg?label=Skidbladnir.Storage.GridFs)](https://www.nuget.org/packages/Skidbladnir.Storage.GridFs/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)
### Description

Implementation of file storage abstraction based on GridFS (file storage in mongodb).  

Well suited for small applications that work in content and do not have their own persistent storage, as well as for storing files that are used by two or more instances.

### Install

For use you needed install packages:
```
Install-Package Skidbladnir.Storage.GridFS
```

### Using

#### Register FileStorage in IoC Container

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddGridFsStorage(new GridFsStorageConfiguration(){ConnectionString = "mongodb://login:pass@localhost:27017/StorageGFTest?authSource=admin"});
    service.AddHostedService<TestBackgroundService>(); //Add sample service
}
```

#### Resolve and use

```c#
public class TestBackgroundService : BackgroundService{
    private IStorage<GridFsStorageInfo> _storage;
    private ILogger<StartupModule> _logger;
    public TestBackgroundService(ILogger<StartupModule> logger , IStorage<GridFsStorageInfo> storage){
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