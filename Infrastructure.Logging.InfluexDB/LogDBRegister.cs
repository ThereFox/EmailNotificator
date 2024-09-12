using Application.Interfaces.Services;
using InfluxDB3.Client;
using Infrastructure.Logging.InfluxDB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistense.Logging.InfluxDB
{
    public static class LogDBRegister
    {
        public static IServiceCollection AddInfluexDBLogging(
                    this IServiceCollection serviceProvider,
                    InfluxConfig config)
        {
            serviceProvider.AddScoped<IInfluxDBClient, InfluxDBClient>(
                creator => new InfluxDBClient(config.Host, config.Token, config.Organisation, config.Bucket)
                );
            serviceProvider.AddScoped<ILogger, InfluexDBLogger>(
                ex =>
                {
                    var service = ex.GetService<IInfluxDBClient>();
                    return new InfluexDBLogger(service, config.Bucket);
                }
            );

            return serviceProvider;
        }
    }
}
