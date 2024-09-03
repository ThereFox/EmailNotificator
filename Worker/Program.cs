using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notificator.Persistense.DI;
using Microsoft.Extensions.Hosting;
using Coravel;
using Autofac.Extensions.DependencyInjection;
using Application;
using Infrastructure.Kafka;
using Infrastructure.Sender;
using Persistense.Logging.InfluxDB;
using Persistense.Notifications.EFCore;

namespace Worker
{
    public class Program
    {
        public static void Main()
        {
            var builder = new HostApplicationBuilder();

            builder.Services
                .AddApp()
                .AddMessageBrockerConsumer()
                .AddNotificatorSender()
                .AddInfluexDBLogging()
                .AddDAL();

            var app = builder.Build();

            app.Services.UseScheduler(
                ex => ex.Schedule<HandleNotificationTask>()
                .EveryFiveMinutes()
                );

            app.Run();
        }
    }
}
