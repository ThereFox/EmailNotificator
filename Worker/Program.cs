using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Hosting;
using Coravel;
using Autofac.Extensions.DependencyInjection;
using Application;
using Infrastructure.Kafka;
using Infrastructure.Sender;
using Persistense.Logging.InfluxDB;
using Persistense.Notifications.EFCore;
using Microsoft.Extensions.Configuration;
using Notification.ConfigsInputObjects;

namespace Worker
{
    public class Program
    {
        public static void Main()
        {
            var builder = Host.CreateApplicationBuilder();

            var config = builder.Configuration.GetSection("Connections").Get<ConnectionsForServices>();

            var consumerConfig = config.Brockers.ConsumerInfo;

            builder.Services.AddScheduler();

            builder.Services
                .AddApp()
                .AddCommandReader(consumerConfig.BrockerUrl, consumerConfig.TopicName, consumerConfig.GroupId)
                .AddNotificatorSender()
                .AddInfluexDBLogging(config.Databases.Logs)
                .AddDAL(config.Databases.Main.ConnectionString);

            var app = builder.Build();

            app.Services.UseScheduler(
                ex => ex.Schedule<HandleNotificationTask>()
                .EveryFiveMinutes()
                );

            app.Run();
        }
    }
}
