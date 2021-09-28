# [Skidbladnir Home](../../../README.md)

## [Caching](../README.md)

## MongoDB distributed cache implementation

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Caching.Distributed.MongoDB.svg?label=Skidbladnir.Caching.Distributed.MongoDB)](https://www.nuget.org/packages/Skidbladnir.Caching.Distributed.MongoDB/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

This library implements a Microsoft distributed cache abstraction using MongoDB as a cache storage and use connection to MongoDB by `Skidbladnir.Repository.MongoDB`.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Repository.MongoDB
Install-Package Skidbladnir.Caching.Distributed.MongoDB
```

### Using

To use the distributed cache, you need to connect the database using the `Skidbladnir.Repository.MongoDB` library and enable distributed cache using extension method `UseMongoDistributedCache`:

```c#
public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            //Add mongodb
            services.AddMongoDbContext(builder =>
                {
                    // Configure Connection string
                    builder.UseConnectionString(configuration.ConnectionString);
                    // Enable MongoDB distributed cache
                    builder.UseMongoDistributedCache(services);
                });
            return services;
        }
```

Or extension method for `IServiceCollection` use `ConfigureMongoDb` for add entity to `BaseMongoDbContext`:

```c#
public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            //Add mongodb
            services.AddMongoDbContext(builder =>
                {
                    // Configure Connection string
                    builder.UseConnectionString(configuration.ConnectionString);
                });
                services.UseMongoDistributedCache();
            return services;
        }
```

Or extension method for `IServiceCollection` use `ConfigureMongoDb<TDbContext>` for add entity to `CustomDbContext`:

```c#
public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            //Add mongodb
            services.AddMongoDbContext<CustomDbContext>(builder =>
                {
                    // Configure Connection string
                    builder.UseConnectionString(configuration.ConnectionString);
                });
                services.UseMongoDistributedCache<CustomDbContext>();
            return services;
        }
```
