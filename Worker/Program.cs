using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Application;
using Infrastructure.Kafka;
using Infrastructure.Sender;
using Persistense.Logging.InfluxDB;
using Persistense.Notifications.EFCore;
using Microsoft.Extensions.Configuration;
using Notification.ConfigsInputObjects;
using System.Runtime.CompilerServices;
using Infrastructure.Brocker.Kafka.Consumer.Service;
using Infrastructure.Brocker.Kafka.Producer;

namespace Worker
{
    public class Program
    {
        public static void Main()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

            var config = builder.Configuration.GetSection("Connections").Get<ConnectionsForServices>();

            var consumerConfig = config.Brockers.ConsumerInfo;
            var producerInfo = config.Brockers.ProducerInfo;

            builder.Services
                .AddApp()
                .AddCommandReader(consumerConfig.BrockerUrl, consumerConfig.TopicName, consumerConfig.GroupId)
                .AddCommandReaderService()
                .AddReportSender(producerInfo.BrockerUrl, producerInfo.TopicName)
                .AddNotificatorSender()
                .AddInfluexDBLogging(config.Databases.Logs)
                .AddDAL(config.Databases.Main.ConnectionString);

            var app = builder.Build();

            app.Run();
        }
    }
}
