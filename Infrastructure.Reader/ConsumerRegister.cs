using System.Net;
using Application.Interfaces.Services;
using Confluent.Kafka;
using Infrastructure.Kafka.Requests;
using Infrastructure.Reader;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class ConsumerRegister
{
    public static IServiceCollection AddMessageBrockerConsumer(this IServiceCollection collection, string brockerUrl)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = brockerUrl,
            AllowAutoCreateTopics = true
        };
        
        var producer = new ConsumerBuilder<Null, SendNotificationCommand>(config)
            .Build();

        
        collection.AddSingleton(producer);

        collection.AddSingleton<IMessageGetter, KafkaConsumer>();

        return collection;
    }
}