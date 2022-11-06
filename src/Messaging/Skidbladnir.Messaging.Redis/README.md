# [Skidbladnir Home](../../../README.md)

## [Messaging](../README.md)

## Messaging Redis

### Description

The `Skidbladnir.Messaging.Redis` module is a message abstraction using Redis as an intermediary.
 
The implementation supports only two work scenarios: Send / Receive and Pub / Sub.

### Install

For use client you only needed install package:

```
Install-Package Skidbladnir.Messaging.Redis
```

### Using

Creating an event class that we will send to the tire and process:

```c#
public class SampleEntity
{
    public long Timestamp { get; set; } = DateTime.UtcNow.ToFileTimeUtc();

    public string Message { get; set; }
}
```

Create event handler :

```c#
public class SampleConsumer : IMessageConsumer<SampleEntity>
{
    private readonly ILogger<SampleConsumer> _logger;

    public SampleConsumer(ILogger<SampleConsumer> logger)
    {
        _logger = logger;
    }

    public Task ConsumeAsync(SampleEntity message, CancellationToken token)
    {
        _logger.LogInformation("Received Message {Message} with timestamp {Time}", message.Message, message.Timestamp);
        return Task.CompletedTask;
    }
}
```

Bus configuration in appsettings.json
```json
{
  "RedisBus": {
    "ConnectionString": "localhost",
    "VirtualHost": "testapp"
  } 
}
```

Next, there are two ways to connect the bus to the project: Through a modular system or directly

#### Via modular system

In `Program.cs` configure Bus settings:
```c#
    .UseSkidbladnirModules<StartupModule>(configuration =>
    {
        configuration.Add(configuration.AppConfiguration.GetSection("RedisBus").Get<RedisBusModuleConfiguration>());
    });
```

Inside `StartupModule.cs` add depended module `RedisBusModule` and register consumers:
```c#
    public class StartupModule : RunnableModule
    {
        public override Type[] DependsModules => new[] {typeof(RedisBusModule)};

        public override void Configure(IServiceCollection services)
        {
            services.ConfigureConsumers()
                .AddConsumer<SampleConsumer>();
        }
    }
```

#### Directly
....


#### Publish event
At now, we can get `IMessageSender` from DI and publish event:

```c#
    public class TestController : ControllerBase
    {
        private readonly IMessageSender _messageSender;
        public TestController(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        [HttpGet]
        public override async Task Get([FromQuery] string text)
        {
            await _messageSender.PublishEventAsync(new DateTimeEvent()
            {
                Message = text
            });
        }
    }

```