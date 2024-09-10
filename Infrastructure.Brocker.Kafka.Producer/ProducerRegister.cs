using Application.Interfaces.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Producer
{
    public static class ProducerRegister
    {
        public static IServiceCollection AddReportSender(this IServiceCollection collection, string brockerURL, string topicName)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = brockerURL,
                AllowAutoCreateTopics = true,
                Acks = Acks.All,
                EnableIdempotence = true
            };

            collection.AddSingleton(new ProducerBuilder<Null, string>(producerConfig).Build());

            collection.AddSingleton<KafkaProducer>();
            collection.AddSingleton<IReportSender, ReportProducer>(
                ex =>
                {
                    var producer = ex.GetService<KafkaProducer>();
                    return new ReportProducer(producer, topicName);
                }
                );

            return collection;
        }
    }
}
