using App.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistense.Notifications.EFCore.Stores;
using Persistense.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Notifications.EFCore
{
    public static class DALRegister
    {
        public static IServiceCollection AddDAL(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<ApplicationDBContext>(
                (ex) => ex.UseNpgsql(connectionString)
            , ServiceLifetime.Singleton
            );

            services.AddSingleton<IBlueprintStore, BlueprintStore>();
            services.AddSingleton<ICustomerStore, CustomerStore>();
            services.AddSingleton<INotificationStore, NotificationStore>();

            return services;
        }
    }
}
