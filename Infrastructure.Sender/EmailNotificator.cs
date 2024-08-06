using Application.Interfaces;
using CSharpFunctionalExtensions;
using Domain.Entitys;

namespace Infrastructure.Sender;

public class EmailNotificator : INotificationHandler
{
    public async Task<Result> Handle(Notification notification)
    {
        return Result.Success();
    }
}
