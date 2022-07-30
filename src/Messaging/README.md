# [Skidbladnir Home](../../README.md)

## Messaging

### Description

Messaging allows you to transfer and coordinate work between various weakly connected components, just as well as the applications that consist of two or more services, for example, divided into a bunch of different services and grocers, distributed not only logically, but also geographically, that perform their specific task.

To support this case, a common tire is well suited for example based on Redis, RabbitMQ or Apache Kafka.

To simplify the work, interfaces are presented that allow you to switch from one implementation of the tire to another with minimal changes in the code.

What does abstraction provide:
* Single event handler interface
  * `ConsumeAsync` is the only method for processing events
* Message sender interface
  * `PublishEventAsync <TEvent>` - sending an event to the bus for all subscribers
  * `SendCommandAsync <TEvent>` - sending a command to a specific service (will be processed with only one copy)

### Use scenarios

There are three main tire use scenarios (Pus / Sub, Send / Receive, Request / Respone). Abstraction supports only two of the three scenarios.

#### Supported scenarios

* `Pub / Sub` - In this scenario, we also have an application / component that publishes messages on the bus, but there is an important difference, all subscribers will receive this message.
* `Send / Receive` - In the bottom scenario, we have applications / components that simply send a message to the bus and no longer follow what happens to it and there are applications / components that receive these messages from the tire and process. It is important that a message taken from a tire is received by only one application / component, t.e. it will not be processed by everyone who listens to the tire, but only by one

#### Unsupported scenarios
* `Request / Response` - In this scenario, when sending a message to the bus, unlike previous scenarios, where there is no feedback on processing the message, here the client will wait for the result from the processor, but the handler will only have one as in (Send / Receive)

### Implementation

* [Messagin.Redis](Skidbladnir.Messaging.Redis/README.md) - Module implementing messaging using Redis as an intermediary