using System.Net;
using Application.Interfaces.Services;
using Confluent.Kafka;
using Infrastructure.Brocker.Kafka.Consumer;
using Infrastructure.Kafka.Requests;
using Infrastructure.Reader;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class ConsumerRegister
{
    public static IServiceCollection AddCommandReader(this IServiceCollection collection, string brockerUrl, string topicName, string groupId)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = brockerUrl,
            GroupId = groupId
        };
        
        var consumer = new ConsumerBuilder<Null, string>(config)
            .Build();

        consumer.Subscribe(topicName);
        
        collection.AddSingleton(consumer);
        collection.AddScoped<KafkaConsumer>();
        collection.AddScoped<IMessageGetter, CommandReader>(ex =>
        {
            var consumer = ex.GetService<KafkaConsumer>();
            return new CommandReader(consumer, topicName);
        });

        return collection;
    }
}