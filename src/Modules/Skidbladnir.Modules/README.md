# [Skidbladnir Home](../../../README.md)
## Simple Modular system

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Modules.svg?label=Skidbladnir.Modules)](https://www.nuget.org/packages/Skidbladnir.Modules/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Table of content
- [Skidbladnir Home](#skidbladnir-home)
  - [Simple Modular system](#simple-modular-system)
    - [Table of content](#table-of-content)
    - [Description](#description)
    - [Install](#install)
    - [Usage](#usage)
      - [Preparation](#preparation)
      - [Integration and launch](#integration-and-launch)
        - [Integration with Hosting](#integration-with-hosting)
        - [Integration with IServiceCollection](#integration-with-iservicecollection)
          - [IServiceCollection of Hosting (for example ConfigureService in Startup.cs aspnet core)](#iservicecollection-of-hosting-for-example-configureservice-in-startupcs-aspnet-core)
          - [IServiceCollection used in console application without Host](#iservicecollection-used-in-console-application-without-host)

### Description

Implementation of a simple system of modules for dotnet applications.   
Contains the base classes of the module and the launched module, methods of integration with `Microsoft.Extensions.Hosting` and simply with`IServiceCollection`, a background service for starting "Launched modules".

Integration takes place as follows:

1. Creation of a starting module in the application (modules from other assemblies will be connected in it, services will be registered, background work will be started)
2. Integration indicating the start module
3. Getting dependent modules from the start module
4. Registering dependencies from all modules
5. Registering the background job launch service
6. Registration of the list of modules in DI.

### Install
For use you needed install packages:
```
Install-Package Skidbladnir.Modules
```

### Usage

#### Preparation

Sample dependent module ( for sample registering LocalStorage):
```c#
public class DependentRunnubleModule: RunnableModule{
    public override void Configure(IServiceCollection services)
    {
        services.AddLocalFsStorage(Configuration["Storage:Path"]);
    }
    
    public override async Task StartAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        var logger = provider.GetService<ILogger<StartupModule>>();
        var localStorage = provider.GetService<IStorage<LocalStorageInfo>>();
        var currentDir = await localStorage.GetFilesAsync("");
        logger.LogInformation("Files in root dir");
        foreach (var fileInfo in currentDir)
        {
            logger.LogInformation("Filename: {FileName}\t\t Length: {Length}\t\t Date: {Date}",
                fileInfo.FileName,
                fileInfo.Size, fileInfo.CreatedDate);
        }
    }
}
```

Sample Startup module:
```c#
public class StartupModule : Module
{
    public override Type[] DependsModules => new[] { typeof(DependentRunnubleModule) };
}
```

#### Integration and launch

##### Integration with Hosting

For integration modular system with host, need use `UseSkidbladnirModules<TModule>` extension on `IHostBuilder`
```c#
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSkidbladnirModules<StartupModule>();
}
```

##### Integration with IServiceCollection

###### IServiceCollection of Hosting (for example ConfigureService in Startup.cs aspnet core)

In this case, under the hood there is a host that can launch IHostedService and you do not need to launch ModuleRunner.
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.UseSkidbladnirModules<StartupModule>();
}
```

###### IServiceCollection used in console application without Host

In this case, there is no host to run the ModuleRunner, so it will need to be started manually
```c#
public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddLogging(b => b.AddConsole());
        services.UseSkidbladnirModules<StartupModule>();
        var provider = services.BuildServiceProvider();
        await provider.StartModules();
    }
```