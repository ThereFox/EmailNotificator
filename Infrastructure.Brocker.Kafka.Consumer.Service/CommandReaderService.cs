using Application;
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
        private readonly NotificationSendService _service;
        private CancellationTokenSource _cancelerTokenSource = new();

        public CommandReaderService(NotificationSendService service)
        {
            _service = service;
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
                await handleMessages();
            }
        }

        private async Task handleMessages()
        {
            var handleNotificationResult = await _service.HandleNotification();
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
