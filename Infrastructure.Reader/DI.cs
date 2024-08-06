using System.Net;
using Application.Interfaces;
using Confluent.Kafka;
using Infrastructure.Kafka.Requests;
using Infrastructure.Reader;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class DI
{
    public static IServiceCollection AddMessageBrocker(this IServiceCollection collection, string brockerUrl)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = brockerUrl,
            AllowAutoCreateTopics = true
        };
        
        var producer = new ConsumerBuilder<Null, SendNotificationRequest>(config)
            .Build();

        
        collection.AddSingleton(producer);

        collection.AddSingleton<IMessageReader, KafkaConsumer>();

        return collection;
    }
}