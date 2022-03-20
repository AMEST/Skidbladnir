using System;

namespace Skidbladnir.Messaging.Abstractions
{
    public interface IMessageConsumerBuilder
    {
         IMessageConsumerBuilder AddConsumer<T>();

         IMessageConsumerBuilder AddConsumer(Type consumerType);
    }
}