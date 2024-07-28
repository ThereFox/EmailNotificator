using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notificator.Persistense.DI;
using Microsoft.Extensions.Hosting;
using Coravel;

namespace Worker
{
    public class Program
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PersistenseModule>();

            var container = builder.Build();

            container.Resolve<IHost>().Services.UseScheduler(
                sheduler => sheduler.Schedule<HandleNotificationTask>()
                .EveryThirtyMinutes()
                );
        }
    }
}
