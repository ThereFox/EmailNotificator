﻿using Application.Interfaces.Services;
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
                creator => new InfluxDBClient(config.host, config.token, config.organisation, config.database)
                );
            serviceProvider.AddScoped<ILogger, InfluexDBLogger>();

            return serviceProvider;
        }
    }
}