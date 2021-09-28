# [Skidbladnir Home](../../../README.md)

## [Repository](../README.md)

## EntityFramework Core repository

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Repository.EntityFrameworkCore.svg?label=Skidbladnir.Repository.EntityFrameworkCore)](https://www.nuget.org/packages/Skidbladnir.Repository.EntityFrameworkCore/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Description

Implementing abstraction of repositories using EntityFramework Core for access to data store.

Registration and configuration of entities context can occur from the `RegisterContext<TDbContext>(dbContextOptionsBuilder => {}, entitiesBuilder => {})` method.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Repository.EntityFrameworkCore
```

### Using

#### Preparation

Sample entity:

```c#
    public class SimpleMessage : IHasId<int>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime? Timestamp { get; set; }
    }

```

Sample entity map:

```c#
    public class SimpleMessageConfiguration : IEntityTypeConfiguration<SimpleMessage>
    {
        public void Configure(EntityTypeBuilder<SimpleMessage> builder)
        {
            builder.ToTable("MessageStore");
        }
    }
```

Sample DbContext (implements `DbContextBase`):

```c#
    public class SampleDbContext : DbContextBase
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
        }

        public DbSet<SimpleMessage> Messages { get; set; }
        public DbSet<SimpleGuid> Guids { get; set; }
    }
```

##### Registration

Next, we register our DbContext, entity and its configuration. We use sqlite as a database.  
Register theirs in `ConfigureServices` method:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.RegisterContext<SampleDbContext>(
        builder => builder.UseSqlite("Data Source=sample.db;Cache=Shared"),
        entities =>
        {
            entities.AddEntity<SimpleMessage, SimpleMessageConfiguration>();
        }
    );
}
```
