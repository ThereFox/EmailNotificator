using Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sender
{
    public static class NotificationSenderRegister
    {
        public static IServiceCollection AddNotificatorSender(this IServiceCollection services)
        {
            return services
                .AddSingleton<INotificationHandler, EmailNotificator>();
        }
    }
}
