using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Brocker.Kafka.Consumer.Service
{
    public static class CommandReaderRegister
    {
        public static IServiceCollection AddCommandReaderService(this IServiceCollection services)
        {
            services.AddHostedService<CommandReaderService>();

            return services;
        }
    }
}
