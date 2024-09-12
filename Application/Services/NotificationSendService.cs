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
            await _logger.LogError(getNewMessageResult.AsError());

            return getNewMessageResult;
        }

        var validateMessageResult = await _validator.CreateByInfo(getNewMessageResult.Value);

        if (validateMessageResult.IsFailure)
        {
            await _logger.LogError(validateMessageResult.AsError());

            await _reportSender.SendReport(getNewMessageResult.Value.Id, true);

            return validateMessageResult;
        }

        var handleResult = await _notificationHandler.Handle(validateMessageResult.Value);

        if (handleResult.IsFailure)
        {
            await _logger.LogError(handleResult.AsError());
        }
        else
        {
            await _logger.LogSendMessage(validateMessageResult.Value);
        }


        var sendReportResult = await _reportSender.SendReport(getNewMessageResult.Value.Id, handleResult.IsFailure);


        return sendReportResult;

    }
}
