using App.Stores;
using CSharpFunctionalExtensions;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Persistense.Entitys;

namespace Persistense.Stores;

public class NotificationStore : INotificationStore
{
    protected readonly ApplicationDBContext _context;

    public NotificationStore(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Result<Notification>> Get(Guid id)
    {
        try
        {
            var notification = await _context
                .Notifications
                .AsNoTracking()
                .Include(ex => ex.Blueprint)
                .Include(ex => ex.Device)
                    .ThenInclude(ex => ex.Owner)
                .FirstOrDefaultAsync(ex => ex.Id == id);

            if (notification == default)
            {
                return Result.Failure<Notification>($"dont contain element with id {id}");
            }

            return notification.ToDomain();
        }
        catch (Exception ex)
        {
            return Result.Failure<Notification>(ex.Message);
        }
    }

    public async Task<Result> SaveNew(Notification notification)
    {
        if(await _context.Database.CanConnectAsync() == false)
        {
            return Result.Failure("Cand connect to database");
        }

        try
        {
            await _context.Notifications.AddAsync(NotificationEntity.FromDomain(notification));
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }

    }
}