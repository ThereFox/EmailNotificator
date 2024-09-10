using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationRegister
    {
        public static IServiceCollection AddApp(this IServiceCollection services)
        {
            return services.AddSingleton<NotificationSendService>()
                .AddSingleton<NotificationCreator>();
        }
    }
}
