# [Skidbladnir Home](../../../README.md)

## [DataProtection](../README.md)

## MongoDB data protection implementation

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.DataProtection.MongoDB.svg?label=Skidbladnir.DataProtection.MongoDB)](https://www.nuget.org/packages/Skidbladnir.DataProtection.MongoDB/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

This library implements a Microsoft DataProtection abstraction using MongoDB as a data protection store and use connection to MongoDB by `Skidbladnir.DataProtection.MongoDB`.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Repository.MongoDB
Install-Package Skidbladnir.DataProtection.MongoDB
```

### Using

To use the data protection, you need to connect the database using the `Skidbladnir.Repository.MongoDB` library and enable distributed cache using extension method `UseDataProtection`:

```c#
public static IServiceCollection AddStorage(this IServiceCollection services)
        {
            //Add mongodb
            services.AddMongoDbContext(builder =>
                {
                    // Configure Connection string
                    builder.UseConnectionString(configuration.ConnectionString);
                    // Enable MongoDB data protection
                    builder.UseDataProtection(services);
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
                // Enable MongoDB data protection
                services.UseDataProtection();
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
                // Enable MongoDB data protection
                services.UseDataProtection<CustomDbContext>();
            return services;
        }
```
