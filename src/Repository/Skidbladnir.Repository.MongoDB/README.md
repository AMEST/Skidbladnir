# [Skidbladnir Home](../../../README.md)

## [Repository](../README.md)

## MongoDB repository

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Repository.MongoDB.svg?label=Skidbladnir.Repository.MongoDB)](https://www.nuget.org/packages/Skidbladnir.Repository.MongoDB/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

Implementing abstraction of repositories using MongoDB as a data store.

The repository can work with one context, or with several (for example, to connect to two databases or to different servers).
When you need to work with only one database, then you do not need to create a separate context, you can use the `AddMongoDbContext (string connectionString, bulder => {})` overload without `<TDbContext>`, in this case, the main MongoContext will be registered. But if you need to work with two or more databases, then you need to create an additional context (inherited from MongoContext) for each database.

Registration and configuration of entities for additional context can occur from the `AddMongoDbContext<TDbContext>(connectionString, builder => {})` method, as well as specifying the type of context through the `ConfigureMongoDbContext<TDbContext>(builder => {})` method to support entity registration, not included in this module or assembly.
Registering and configuring entities for the main context is the same as for the additional one, but only without explicitly specifying `<TDbContext>`.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Repository.MongoDB
```

### Using

#### Preparation

Sample entity:

```c#
public class FeedItem: IHasId<string>
{
    public string Id {get; set;}
    public string Title { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public DateTime PubDate { get; set; }
    public string ChannelName { get; set; }
}

```

Sample entity map:

```c#
public class FeedItemMap : EntityMapClass<FeedItem>
{
    public FeedItemMap()
    {
        ToCollection("FeedItems");
        MapId( x => x.Id);
    }
}
```

##### Registration

Next, we have two ways to register our context and entity:

1. Registering the main context
1. Registration of additional context

In both cases, we will write the following in the module in ConfigureServices:
Registering the main BaseMongoDbContext and the entity with configuration:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMongoDbContext("mongodb://user:password@mongodb/sampledb");
    services.ConfigureMongoDbContect(builder =>{
        builder.AddEntity<FeedItem, FeedItemMap>();
    });
}
```

Registration of an additional `ExampleMongoContext` (inherited from BaseMongoDbContext) with automatic entity configuration:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMongoDbContext<ExampleMongoContext>("mongodb://user:password@mongodb/sampledb2", 
    builder =>{
        builder.AddEntity<FeedItem>();
    });
}
```
