using Application;
using Coravel.Invocable;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class HandleNotificationTask : IInvocable
    {
        private readonly NotificationSendService _service;

        public HandleNotificationTask(NotificationSendService service)
        {
            _service = service;
        }

        public async Task Invoke()
        {
            while (true)
            {
                var handleNotificationResult = await _service.HandleNotification();

                if (handleNotificationResult.IsFailure)
                {
                    break;
                }
            }

        }

    }
}
