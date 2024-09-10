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
    private readonly IReportSender _reportSender;

    public NotificationSendService(
        IMessageGetter getter,
        INotificationHandler handler,
        NotificationCreator validator,
        ILogger logger,
        IReportSender reportSender)
    {
        _messageService = getter;
        _notificationHandler = handler;
        _validator = validator;
        _logger = logger;
        _reportSender = reportSender;
    }

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

            await _reportSender.SendReport(getNewMessageResult.Value.Id, true);

            return error.AsResult();
        }

        var handleResult = await _notificationHandler.Handle(validateMessageResult.Value);

        if (handleResult.IsFailure)
        {
            var error = new Error($"Error while handling new message: {validateMessageResult.Error}");

            await _logger.LogError(error);
        }
        else
        {
            await _logger.LogSendMessage(validateMessageResult.Value);
        }


        var sendReportResult = await _reportSender.SendReport(getNewMessageResult.Value.Id, handleResult.IsFailure);


        return sendReportResult;

    }
}
