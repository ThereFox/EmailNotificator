using Application.Interfaces;
using Autofac.Core;
using CSharpFunctionalExtensions;

namespace Application;

public class NotificationSendService
{
    private readonly IMessageReader _messageService;
    private readonly INotificationHandler _notificationHandler;

    public async Task<Result> HandleNotification()
    {
        var checkMessageHasResult = await _messageService.HasMessages();

        if (checkMessageHasResult.IsFailure)
        {
            //log
            return Result.Failure($"Error while checkMessage: {checkMessageHasResult.Error}");
        }

        if (checkMessageHasResult.Value == false)
        {
            return Result.Failure("dont contain notification");
        }

        var handleResult = await handleNotification();

        return Result.SuccessIf(handleResult.IsSuccess, handleResult.Error);
    }
    protected async Task<Result> handleNotification()
    {
        var getMessageResult = await _messageService.GetNewMessage();

        if (getMessageResult.IsFailure)
        {
            return Result.Failure($"error while getting message {getMessageResult.Error}");
        }

        var message = getMessageResult.Value;

        var handleNotificationResult = await _notificationHandler.Handl(message);

        return Result.SuccessIf(handleNotificationResult.IsSuccess, handleNotificationResult.Error);
    }
}
