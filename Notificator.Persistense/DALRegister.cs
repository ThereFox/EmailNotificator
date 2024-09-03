using App.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            , ServiceLifetime.Scoped
            );

            services.AddTransient<IBlueprintStore, BlueprintStore>();
            services.AddTransient<ICustomerStore, CustomerStore>();
            services.AddTransient<INotificationStore, NotificationStore>();

            return services;
        }
    }
}
