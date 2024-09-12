using Microsoft.Extensions.DependencyInjection;
using Persistense.Cache.Notifications.DI;
using Persistense.Notifications.Cache.Stores;
using Persistense.Notifications.EFCore.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Notifications.Cache
{
    public static class CacheRegister
    {

        public static IServiceCollection AddRedisCache(this IServiceCollection services, string Host, int Port, string UserName, string UserPassword)
        {
            services.AddSingleton<CacheConnectionGetter>(new CacheConnectionGetter(Host, Port, UserName, UserPassword));
            services.AddScoped<IConnectionMultiplexer>(
                ex =>
                {
                    var factory = ex.GetService<CacheConnectionGetter>();
                    return factory.GetConnection();
                }
                );
            services.AddScoped<IDatabase>(ex =>
            {
                var factory = ex.GetService<IConnectionMultiplexer>();
                return factory.GetDatabase();
            });
            services.AddScoped<IBlueprintCacheStore, BlueprintCacheStore>();

            return services;
        }

    }
}
