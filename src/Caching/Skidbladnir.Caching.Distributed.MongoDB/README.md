# [Skidbladnir Home](../../../README.md)

## [Caching](../README.md)

## MongoDB distributed cache implementation

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Caching.Distributed.MongoDB.svg?label=Skidbladnir.Caching.Distributed.MongoDB)](https://www.nuget.org/packages/Skidbladnir.Caching.Distributed.MongoDB/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

This library implements a Microsoft distributed cache abstraction using MongoDB as a cache storage.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Modules
Install-Package Skidbladnir.Caching.Distributed.MongoDB
```

### Using

To use the distributed cache, you need execute extension method `AddMongoDistributedCache(string connectionString)` or `AddMongoDistributedCache(DistributedCacheMongoModuleConfiguration configuration)` on `IServiceCollection`:

##### Example add Distributed Cache in `Startup.cs`
```c#
public static void ConfigureServices(this IServiceCollection services)
{
    services.AddMongoDistributedCache("ConnectionString");
}
```

##### Example add Distributed Cache in module system `StartupModule.cs`

In `programm.cs` configure `Skidbladnir.Modules`:

```c#
private static IHostBuilder CreateHostBuilder(string[] args) =>
    Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .UseSkidbladnirModules<StartupModule>(configuration =>
        {
            var distributedCacheConfig = configuration.AppConfiguration.GetSection("ConnectionStrings:Mongo").Get<DistributedCacheMongoModuleConfiguration>();
            configuration.Add(distributedCacheConfig);
        });
```

And enable module `DistributedCacheMongoModule` inside `StartupModule.cs`:

```c#
public class StartupModule : Module
{
    public override Type[] DependsModules => new []{ typeof(DistributedCacheMongoModule) };

}
```