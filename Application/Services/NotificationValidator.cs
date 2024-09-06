using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;
using Infrastructure.Kafka.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationCreator
    {
        private readonly ICustomerStore _customerStore;
        private readonly IBlueprintStore _blueprintStore;

        public async Task<Result<Notification>> CreateByInfo(SendNotificationInputObject request)
        {
            var blueprintCreateResult = await CreateBlueprint(request.BlueprintId);

            if (blueprintCreateResult.IsFailure)
            {
                return blueprintCreateResult.ConvertFailure<Notification>();
            }

            var getDeviceResult = await GetDeviceByCustomer(request.CustomerId, blueprintCreateResult.Value.Channel);

            if (getDeviceResult.IsFailure)
            {
                return getDeviceResult.ConvertFailure<Notification>();
            }

            var result = Notification.Create(
                Guid.NewGuid(),
                getDeviceResult.Value,
                blueprintCreateResult.Value,
                NotificationStatus.Created,
                DateTime.Now,
                default);

            return result;
        }
        protected async Task<Result<Device>> GetDeviceByCustomer(Guid CustomerId, NotificationChannel channel)
        {
            var getCustomerResult = await _customerStore.Get(CustomerId);

            if (getCustomerResult.IsFailure)
            {
                return Result.Failure<Device>("Cant get customer");
            }

            var pickDeviceResult = getCustomerResult.Value.PickDeviceForNotificationByChannel(channel);

            return pickDeviceResult;
        }

        protected async Task<Result<Blueprint>> CreateBlueprint(Guid BlueprintId)
        {
            return await _blueprintStore.Get(BlueprintId);
        }
    }
}
