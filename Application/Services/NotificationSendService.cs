using Application.Interfaces.Services;
using Application.Services;
using Autofac.Core;
using Common;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Infrastructure.Kafka.Requests;

namespace Application;

public class NotificationSendService
{
    private readonly IMessageGetter _messageService;
    private readonly INotificationHandler _notificationHandler;
    private readonly NotificationCreator _validator;
    private readonly ILogger _logger;

    public async Task<Result> HandleNotification()
    {
        var getNewMessageResult = await _messageService.GetNewMessage();

        if (getNewMessageResult.IsFailure)
        {
            var error = new Error($"Error while getting new message: {getNewMessageResult.Error}");

            await _logger.LogError(error);

            return error.AsResult();
        }

        var validateMessageResult = await _validator.CreateByInfo(getNewMessageResult.Value);

        if (validateMessageResult.IsFailure)
        {
            var error = new Error($"Error while validating new message: {validateMessageResult.Error}");

            await _logger.LogError(error);

            return error.AsResult();
        }

        var handleResult = await _notificationHandler.Handle(validateMessageResult.Value);

        if (handleResult.IsFailure)
        {
            var error = new Error($"Error while handling new message: {validateMessageResult.Error}");

            await _logger.LogError(error);
        }

        return handleResult;

    }
}
