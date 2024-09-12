using Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Brocker.Kafka.Consumer.Service
{
    public class CommandReaderService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private CancellationTokenSource _cancelerTokenSource = new();

        public CommandReaderService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(consumeLoop, _cancelerTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return Task.CompletedTask;
        }

        private async Task consumeLoop()
        {
            while (_cancelerTokenSource.IsCancellationRequested == false)
            {
                await handleMessagesInNewScope();
            }
        }

        private async Task handleMessagesInNewScope()
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var sendService = scope.ServiceProvider.GetService<NotificationSendService>();

                await handleMessages(sendService);
            }
        }

        private async Task handleMessages(NotificationSendService service)
        {
            var handleNotificationResult = await service.HandleNotification();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_cancelerTokenSource.IsCancellationRequested)
            {
                return;
            }

            await _cancelerTokenSource.CancelAsync();
        }
    }
}
