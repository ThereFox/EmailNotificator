using CSharpFunctionalExtensions;
using Domain.Entitys;
using Domain.ValueObject;

namespace Persistense.Entitys;

public class NotificationEntity
{
    public Guid Id { get; set; }
    
    public Guid DeviceId { get; set; }
    public Guid BlueprintId { get; set; }
    
    public int Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? SendAt { get; set; }
    
    public DeviceSettingsEntity Device { get; set; }
    public NotificationBlueprintEntity Blueprint { get; set; }
    
    public static NotificationEntity FromDomain(Notification notification)
    {
        return new NotificationEntity()
        {
            Id = notification.Id,
            BlueprintId = notification.Blueprint.Id,
            CreatedAt = notification.CreatedAt,
            DeviceId = notification.DeviceResiver.Id,
            SendAt = notification.SendedAt,
            Status = notification.Status.Value
        };
    }
    public Result<Notification> ToDomain()
    {
        var validateStatus = NotificationStatus.Create(Status);

        if (validateStatus.IsFailure)
        {
            return Result.Failure<Notification>(validateStatus.Error);
        }

        var validateBlueprint = Blueprint is not null ? Blueprint.ToDomain() : Result.Failure<Blueprint>("empty blueprint");
        var validateDevice = Device is not null ? Device.ToDomain() : Result.Failure<Device>("empty device");

        return Notification.Create(
            Id,
            validateDevice.IsFailure ? null : validateDevice.Value,
            validateBlueprint.IsFailure ? null : validateBlueprint.Value,
            validateStatus.Value,
            CreatedAt,
            SendAt
            );

    }

}