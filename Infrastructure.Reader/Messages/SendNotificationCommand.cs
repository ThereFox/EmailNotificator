namespace Infrastructure.Kafka.Requests;

public record SendNotificationCommand
(
    Guid Id,
    Guid BlueprintId,
    Guid CustomerId
);