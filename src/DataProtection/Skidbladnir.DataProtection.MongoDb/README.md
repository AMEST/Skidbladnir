# [Skidbladnir Home](../../../README.md)

## [DataProtection](../README.md)

## MongoDB data protection implementation

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.DataProtection.MongoDB.svg?label=Skidbladnir.DataProtection.MongoDB)](https://www.nuget.org/packages/Skidbladnir.DataProtection.MongoDB/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

This library implements a Microsoft DataProtection abstraction using MongoDB as a data protection store.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Modules
Install-Package Skidbladnir.DataProtection.MongoDB
```

### Using

To use the data protection, you need enable data protection using extension method `PersistKeysToMongoDb(string connectionString, string collectionName = null)` on `IDataProtectionBuilder` or `AddDataProtectionMongoDb(string connectionString, string collectionName = null)` on `IServiceCollection`.

##### Enable Data Protection MongoDb in `Startup.cs`

`PersistKeysToMongoDb`:

```c#
public void ConfigureServices(this IServiceCollection services)
{
    services.AddDataProtection()
        .PersistKeysToMongoDb("ConnectionString");
}
```

`IServiceCollection`:

```c#
public void ConfigureServices(this IServiceCollection services)
{
    services.AddDataProtection();
    services.AddDataProtectionMongoDb("ConnectionString");
}
```