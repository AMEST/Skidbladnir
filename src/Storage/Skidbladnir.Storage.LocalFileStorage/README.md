# [Skidbladnir Home](../../../README.md)

## [Storage](../README.md)

## LocalFS storage

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Storage.LocalFileStorage.svg?label=Skidbladnir.Storage.LocalFileStorage)](https://www.nuget.org/packages/Skidbladnir.Storage.LocalFileStorage/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

An implementation of the file storage abstraction on the file system of the host on which the application is running.  
Well suited for small applications that work only in one instance, as well as for the implementation of a file cache, where you can temporarily store data received, for example, from the GridFS storage module.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Storage.LocalFileStorage
```

### Using

#### Register FileStorage in IoC Container

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddLocalFsStorage(new LocalFsStorageConfiguration(){Path ="c:\\file_storage"});
    service.AddHostedService<TestBackgroundService>(); //Add sample service
}
```

#### Resolve and use

```c#
public class TestBackgroundService : BackgroundService{
    private IStorage<LocalStorageInfo> _localStorage;
    private ILogger<StartupModule> _logger;
    public TestBackgroundService(ILogger<StartupModule> logger , IStorage<LocalStorageInfo> storage){
        _localStorage = storage;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentDir = await _localStorage.GetFilesAsync("");
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
